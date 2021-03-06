﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using log4net;

namespace CarpoolPlanner.Model
{
    /// <summary>
    /// A database context for accessing the CarpoolPlanner database.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public static readonly TimeSpan TripInstanceRemovalDelay = TimeSpan.FromHours(1); // TODO: make this a DB setting? maybe a property of Trip.
        private static object tripInstanceCreationLock = new object();

        private static readonly ILog log = LogManager.GetLogger(typeof(ApplicationDbContext));

        public ApplicationDbContext()
            : base("LocalMySql")
        {
            // All dates should be stored as UTC in the database. As such, specify the DateTimeKind when loading them.
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, e) =>
            {
                var entity = e.Entity;
                if (entity == null)
                    return;

                var properties = entity.GetType().GetProperties()
                    .Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    DateTime? dt = null;
                    if (property.PropertyType == typeof(DateTime))
                        dt = (DateTime)property.GetValue(entity);
                    else if (property.PropertyType == typeof(DateTime?))
                        dt = (DateTime?)property.GetValue(entity);

                    if (dt == null)
                        continue;

                    property.SetValue(entity, DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc));
                }
            };
        }

        [Obsolete("Use IDbContextProvider.GetContext() instead.")]
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Trip> Trips { get; set; }

        public IDbSet<TripRecurrence> TripRecurrences { get; set; }

        public IDbSet<TripInstance> TripInstances { get; set; }

        public IDbSet<UserTrip> UserTrips { get; set; }

        public IDbSet<UserTripRecurrence> UserTripRecurrences { get; set; }

        public IDbSet<UserTripInstance> UserTripInstances { get; set; }

        public IDbSet<Log> Logs { get; set; }

        /// <summary>
        /// Gets all UserTrips for the specified user.
        /// </summary>
        /// <returns>A <see cref="IQueryable{UserTrip}"/>.</returns>
        public IQueryable<UserTrip> GetUserTrips(long userId)
        {
            return UserTrips.Where(ut => ut.UserId == userId);
        }

        /// <summary>
        /// Gets all UserTripRecurrences for the specified user.
        /// </summary>
        /// <returns>A <see cref="IQueryable{UserTrip}"/>.</returns>
        public IQueryable<UserTripRecurrence> GetUserTripRecurrences(long userId)
        {
            return UserTripRecurrences.Where(ut => ut.UserId == userId);
        }

        /// <summary>
        /// Gets the trip instance with the specified ID using the eager loading required to calculate statistics (such as seats available).
        /// </summary>
        /// <param name="tripInstanceId">The ID of the TripInstance to get.</param>
        /// <returns>The requested TripInstance.</returns>
        public TripInstance GetTripInstanceById(long tripInstanceId)
        {
            // Unfortunately this can't be done in one query because mono has bugs or something (it works fine in .NET, though).
            /*return TripInstances
                .Include(ti => ti.Trip)
                .Include(ti => ti.UserTripInstances.Select(uti => uti.User))
                .Include(ti => ti.UserTripInstances.Select(uti => uti.UserTrip))
                .FirstOrDefault(ti => ti.Id == tripInstanceId);*/
            var tripInstance = TripInstances
                .Include(ti => ti.UserTripInstances.Select(uti => uti.UserTrip))
                .FirstOrDefault(ti => ti.Id == tripInstanceId);
            if (tripInstance == null)
                return null;
            Trips.Find(tripInstance.TripId);
            foreach (var userTripInstance in tripInstance.UserTripInstances)
            {
                Users.Find(userTripInstance.UserId);
            }
            return tripInstance;
        }

        /// <summary>
        /// Gets the next trip instance for the recurrence (that is not skipped) after the current date. If the instance does not yet exist then it is created.
        /// </summary>
        /// <param name="recurrence">The TripRecurrence for which to get the next instance.</param>
        /// <param name="delay">The amout of time after the instance occurs that the instance is still considered the "next" instance.</param>
        /// <returns>The next TripInstance.</returns>
        public TripInstance GetNextTripInstance(TripRecurrence recurrence, TimeSpan delay)
        {
            if (recurrence == null)
                return null;
            var trip = Trips.Find(recurrence.TripId);
            var timeZone = trip.DateTimeZone;
            lock (tripInstanceCreationLock)
            {
                var expectedDate = recurrence.GetNextInstanceDate(DateTime.UtcNow - delay, timeZone);
                if (expectedDate == null)
                    return null;
                if (expectedDate < DateTime.UtcNow - delay)
                {
                    log.Warn("expectedDate is before now. This probably indicates a bug.");
                    return null;
                }
                var instance = TripInstances.FirstOrDefault(ti => ti.TripId == recurrence.TripId && ti.Date == expectedDate);
                while (instance != null && instance.Skip)
                {
                    expectedDate = recurrence.GetNextInstanceDate(expectedDate.Value, timeZone);
                    if (expectedDate == null)
                        return null;
                    instance = TripInstances.FirstOrDefault(ti => ti.TripId == recurrence.TripId && ti.Date == expectedDate);
                }
                if (instance != null)
                    return instance;
                var tripInstance = CreateTripInstance(recurrence, expectedDate.Value);
                return tripInstance;
            }
        }

        private TripInstance CreateTripInstance(TripRecurrence recurrence, DateTime date)
        {
            var tripInstance = new TripInstance { TripId = recurrence.TripId, Date = date };
            TripInstances.Add(tripInstance);
            // Automatically add any users who are attending the TripRecurrence to this instance.
            foreach (var userTripRecurrence in UserTripRecurrences.Include(utr => utr.User).Where(utr => utr.TripRecurrenceId == recurrence.Id))
            {
                var userTripInstance = UserTripInstance.Create(userTripRecurrence.User, tripInstance);
                userTripInstance.Attending = userTripRecurrence.Attending ? (bool?)null : false;
                UserTripInstances.Add(userTripInstance);
            }
            SaveChanges();
            return tripInstance;
        }

        /// <summary>
        /// Gets the next trip instance for the specified user for the recurrence (that is not skipped) after the current date. If the instance does not yet exist then it is created.
        /// </summary>
        /// <param name="recurrence">The TripRecurrence for which to get the next instance.</param>
        /// <returns>The next UserTripInstance.</returns>
        public UserTripInstance GetNextUserTripInstance(TripRecurrence recurrence, User user, TimeSpan delay)
        {
            var instance = GetNextTripInstance(recurrence, delay);
            if (instance == null)
                return null;
            // Note: there could be a multithreading issue here if the same user logs in from 2 different clients at once.
            // However that is extremely unlikely and I'm too lazy to fix it.
            var userTripInstance = UserTripInstances.FirstOrDefault(uti => uti.TripInstanceId == instance.Id && uti.UserId == user.Id);
            if (userTripInstance != null)
                return userTripInstance;
            userTripInstance = UserTripInstance.Create(user, instance);
            var userTripRecurrence = recurrence.UserTripRecurrences[user.Id];
            userTripInstance.Attending = (userTripRecurrence != null && userTripRecurrence.Attending) ? (bool?)null : false;
            userTripInstance.User = null;
            UserTripInstances.Add(userTripInstance);
            return userTripInstance;
        }

        public User GetUserByPhoneNumber(string phone)
        {
            phone = NormalizePhoneNumber(phone);
            foreach (var user in Users.Where(u => !string.IsNullOrEmpty(u.Phone)))
            {
                var userPhone = NormalizePhoneNumber(user.Phone);
                if (userPhone == phone)
                    return user;
            }
            return null;
        }

        private static readonly Regex ignorePhoneChars = new Regex("[^0-9x]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string NormalizePhoneNumber(string phone)
        {
            if (phone == null)
                return null;
            phone = ignorePhoneChars.Replace(phone, "");
            var phoneWithoutExtension = phone;
            var extensionIndex = phone.IndexOf("x");
            if (extensionIndex != -1)
                phoneWithoutExtension = phone.Remove(extensionIndex);
            if (phoneWithoutExtension.Length == 10)
                phone = "1" + phone; // Assume country code of 1
            phone = "+" + phone;
            return phone;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(c => c.ToTable(GetDbObjectName(c.ClrType.Name)));
            modelBuilder.Properties().Configure(c => c.HasColumnName(GetDbObjectName(c.ClrPropertyInfo.Name)));

            modelBuilder.Entity<UserTripInstance>()
                .HasRequired(uti => uti.UserTrip)
                .WithMany(ut => ut.Instances)
                .HasForeignKey(uti => new { uti.UserId, uti.TripId })
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }

        private string GetDbObjectName(string clrName)
        {
            // Add an underscore before each capital letter (but not before the first letter) to get the object name.
            var result = Regex.Replace(clrName, ".[A-Z]", m => m.Value[0] + "_" + m.Value[1]);
            // Table names in MySql are all lower case.
            return result.ToLowerInvariant();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Log the exception and re-throw it
                var sb = new StringBuilder();
                sb.AppendLine("Validation errors occured while saving entities:");
                foreach (var eve in ex.EntityValidationErrors.Where(e => !e.IsValid))
                {
                    foreach (var dve in eve.ValidationErrors)
                    {
                        sb.Append(dve.PropertyName);
                        sb.Append(": ");
                        sb.AppendLine(dve.ErrorMessage);
                    }
                }
                log.Error(sb.ToString());
                throw;
            }
        }
    }
}
