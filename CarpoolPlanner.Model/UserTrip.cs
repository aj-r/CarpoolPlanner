using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CarpoolPlanner.Model
{
    public class UserTrip
    {
        public UserTrip()
        {
            Recurrences = new List<UserTripRecurrence>();
            Instances = new List<UserTripInstance>();
        }

        [Key, Column("user_id", Order = 0)]
        public long UserId { get; set; }

        [Key, Column("trip_id", Order = 1)]
        public long TripId { get; set; }

        public bool Attending { get; set; }

        [ForeignKey("TripId")]
        [JsonIgnore]
        public Trip Trip { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<UserTripRecurrence> Recurrences { get; private set; }

        [JsonIgnore]
        public ICollection<UserTripInstance> Instances { get; private set; }

        /// <summary>
        /// Gets the ratio of the number of times the user has driven recently to the number of times they have attended.
        /// </summary>
        public double GetDrivingRatio()
        {
            // TODO: eventually delete old trip instances (how old?)
            int total = Instances.Count(uti => uti.Attending == true && uti.CommuteMethod != CommuteMethod.HaveRide);
            int driveCount = Instances.Count(uti => uti.Attending == true && uti.CommuteMethod == CommuteMethod.Driver);
            return (double)driveCount / (double)total;
        }
    }
}