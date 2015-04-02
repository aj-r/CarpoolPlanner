using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpoolPlanner.Model
{
    public interface IApplicationDbContext : IDisposable
    {
        IDbSet<User> Users { get; }

        IDbSet<Trip> Trips { get; }

        IDbSet<TripRecurrence> TripRecurrences { get; }

        IDbSet<TripInstance> TripInstances { get; }

        IDbSet<UserTrip> UserTrips { get; }

        IDbSet<UserTripRecurrence> UserTripRecurrences { get; }

        IDbSet<UserTripInstance> UserTripInstances { get; }

        IDbSet<Log> Logs { get; }

        /// <summary>
        /// Gets all UserTrips for the specified user.
        /// </summary>
        /// <returns>An IEnumerable&lt;UserTrip&gt;.</returns>
        IQueryable<UserTrip> GetUserTrips(long userId);

        /// <summary>
        /// Gets all UserTripRecurrences for the specified user.
        /// </summary>
        /// <returns>An IEnumerable&lt;UserTrip&gt;.</returns>
        IQueryable<UserTripRecurrence> GetUserTripRecurrences(long userId);

        /// <summary>
        /// Gets the trip instance with the specified ID using the eager loading required to calculate statistics (such as seats available).
        /// </summary>
        /// <param name="tripInstanceId">The ID of the TripInstance to get.</param>
        /// <returns>The requested TripInstance.</returns>
        TripInstance GetTripInstanceById(long tripInstanceId);

        /// <summary>
        /// Gets the next trip instance for the recurrence (that is not skipped) after the current date. If the instance does not yet exist then it is created.
        /// </summary>
        /// <param name="recurrence">The TripRecurrence for which to get the next instance.</param>
        /// <param name="delay">The amout of time after the instance occurs that the instance is still considered the "next" instance.</param>
        /// <returns>The next TripInstance.</returns>
        TripInstance GetNextTripInstance(TripRecurrence recurrence, TimeSpan delay);

        /// <summary>
        /// Gets the next trip instance for the specified user for the recurrence (that is not skipped) after the current date. If the instance does not yet exist then it is created.
        /// </summary>
        /// <param name="recurrence">The TripRecurrence for which to get the next instance.</param>
        /// <returns>The next UserTripInstance.</returns>
        UserTripInstance GetNextUserTripInstance(TripRecurrence recurrence, User user, TimeSpan delay);

        User GetUserByPhoneNumber(string phone);

        int SaveChanges();
    }
}
