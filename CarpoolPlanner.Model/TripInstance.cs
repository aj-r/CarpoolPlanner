using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace CarpoolPlanner.Model
{
    public class TripInstance
    {
        public TripInstance()
        {
            UserTripInstances = new List<UserTripInstance>();
        }

        [Key]
        public long Id { get; set; }

        public long TripId { get; set; }

        public DateTime Date { get; set; }

        public bool Skip { get; set; }

        public Trip Trip { get; set; }

        public bool DriversPicked { get; set; }

        /// <summary>
        /// Gets or sets the list of UserTripInstances that belong to this trip instance.
        /// </summary>
        public IList<UserTripInstance> UserTripInstances { get; private set; }

        public int GetRequiredSeats()
        {
            var seatsRequired = UserTripInstances.Count(uti => uti.User.Status == UserStatus.Active && uti.Attending == true && uti.CommuteMethod != CommuteMethod.HaveRide);
            return seatsRequired;
        }

        public int GetAvailableSeats()
        {
            var seatsAvailable = UserTripInstances.Sum(uti => (uti.User.Status == UserStatus.Active && uti.Attending == true && uti.CommuteMethod == CommuteMethod.Driver) ? uti.Seats : 0);
            return seatsAvailable;
        }

        public string GetStatusReport()
        {
            var drivers = (from uti in UserTripInstances
                           where uti.Attending == true && uti.CommuteMethod == CommuteMethod.Driver && uti.User.Status == UserStatus.Active
                           select uti.User.Name ?? uti.UserId).ToList();
            var passengers = (from uti in UserTripInstances
                              where uti.Attending == true && uti.CommuteMethod == CommuteMethod.NeedRide && uti.User.Status == UserStatus.Active
                              select uti.User.Name ?? uti.UserId).ToList();
            var kickedUsers = (from uti in UserTripInstances
                               where uti.Attending == false && uti.NoRoom && uti.User.Status == UserStatus.Active
                               select uti.User.Name ?? uti.UserId).ToList();
            var unconfirmed = (from uti in UserTripInstances
                               where uti.Attending == null && uti.User.Status == UserStatus.Active
                               select uti.User.Name ?? uti.UserId).ToList();
            var sb = new StringBuilder(75);
            if (drivers.Count > 0)
            {
                sb.Append("\nDrivers:\n");
                foreach (var userName in drivers)
                {
                    sb.Append(userName + "\n");
                }
            }
            if (passengers.Count > 0)
            {
                sb.Append("\nPassengers:\n");
                foreach (var userName in passengers)
                {
                    sb.Append(userName + "\n");
                }
            }
            if (kickedUsers.Count > 0)
            {
                sb.Append("\nWaiting list:\n");
                foreach (var userName in kickedUsers)
                {
                    sb.Append(userName + "\n");
                }
            }
            if (unconfirmed.Count > 0)
            {
                sb.Append("\nUnconfirmed:\n");
                foreach (var userName in unconfirmed)
                {
                    sb.Append(userName + "\n");
                }
            }
            return sb.ToString().Trim('\n');
        }

        public override string ToString()
        {
            var datePart = Date.Date;
            if (datePart == DateTime.Today)
                return Date.ToString("dddd, MMM d '(today) at' h:mm tt");
            else if (datePart == DateTime.Today.AddDays(1))
                return Date.ToString("dddd, MMM d '(tomorrow) at' h:mm tt");
            else
                return Date.ToString("dddd, MMM d 'at' h:mm tt");
        }
    }
}