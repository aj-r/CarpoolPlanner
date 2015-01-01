using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CarpoolPlanner.Model
{
    public class UserTripInstance
    {
        public static UserTripInstance Create(User user, TripInstance tripInstance)
        {
            return new UserTripInstance
            {
                UserId = user.Id,
                User = user,
                TripInstanceId = tripInstance.Id,
                TripInstance = tripInstance,
                TripId = tripInstance.TripId,
                CommuteMethod = user.CommuteMethod,
                CanDriveIfNeeded = user.CanDriveIfNeeded || user.CommuteMethod == CommuteMethod.Driver,
                Seats = user.Seats
            };
        }

        [Key, Column(Order = 0)]
        public long UserId { get; set; }

        [Key, Column(Order = 1)]
        public long TripInstanceId { get; set; }

        public long TripId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("TripInstanceId")]
        [JsonIgnore]
        public TripInstance TripInstance { get; set; }

        [JsonIgnore]
        public UserTrip UserTrip { get; set; }

        public bool? Attending { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user confirmed that they are attending (only applies if Attending or NoRoom is true).
        /// </summary>
        public DateTime? ConfirmTime { get; set; }

        /// <summary>
        /// Gets or sets a value that, when true, indicates that the user wants to come, but there are not enough seats.
        /// </summary>
        public bool NoRoom { get; set; }

        public CommuteMethod CommuteMethod { get; set; }

        /// <summary>
        /// Indicates that the user can drive other people if necessary, but would prefer not to. Does not apply if CommuteMethod is Driver.
        /// </summary>
        public bool CanDriveIfNeeded { get; set; }

        public int Seats { get; set; }

        [MaxLength(5000)]
        public string Note { get; set; }

    }
}