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
        public UserTripInstanceCollection UserTripInstances { get; private set; }

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

        public int GetMaxAvailableSeats()
        {
            var seatsAvailable = UserTripInstances.Sum(uti => (uti.User.Status == UserStatus.Active
                && uti.Attending == true
                && (uti.CommuteMethod == CommuteMethod.Driver || uti.CanDriveIfNeeded))
                ? uti.Seats : 0);
            return seatsAvailable;
        }
    }
}