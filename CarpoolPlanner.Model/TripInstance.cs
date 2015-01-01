using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace CarpoolPlanner.Model
{
    public class TripInstance
    {
        public TripInstance()
        {
            UserTripInstances = new UserTripInstanceCollection();
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
        [JsonIgnore]
        public UserTripInstanceCollection UserTripInstances { get; private set; }

        // TODO: remove these 2 functions
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

        /// <summary>
        /// Gets a string that lists the statuses of all users who are attending (or who want to attend).
        /// </summary>
        /// <returns>A string.</returns>
        public string GetStatusReport()
        {
            var drivers = (from uti in UserTripInstances
                           where uti.Attending == true && uti.CommuteMethod == CommuteMethod.Driver && uti.User.Status == UserStatus.Active
                           select uti.User.Name ?? uti.User.LoginName).ToList();
            var passengers = (from uti in UserTripInstances
                              where uti.Attending == true && uti.CommuteMethod == CommuteMethod.NeedRide && uti.User.Status == UserStatus.Active
                              select uti.User.Name ?? uti.User.LoginName).ToList();
            var kickedUsers = (from uti in UserTripInstances
                               where uti.Attending == false && uti.NoRoom && uti.User.Status == UserStatus.Active
                               select uti.User.Name ?? uti.User.LoginName).ToList();
            var unconfirmed = (from uti in UserTripInstances
                               where uti.Attending == null && uti.User.Status == UserStatus.Active
                               select uti.User.Name ?? uti.User.LoginName).ToList();
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
    }
}