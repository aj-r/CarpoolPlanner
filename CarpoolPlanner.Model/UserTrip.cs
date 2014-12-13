using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        public string UserId { get; set; }

        [Key, Column("trip_id", Order = 1)]
        public long TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<UserTripRecurrence> Recurrences { get; private set; }

        public ICollection<UserTripInstance> Instances { get; private set; }

        /// <summary>
        /// Gets the ratio of the number of times the user has driven recently to the number of times they have attended.
        /// </summary>
        public double GetDrivingRatio()
        {
            if (Instances.Count == 0)
            {
                using (var context = ApplicationDbContext.Create())
                {
                    // TODO: test to make sure this works
                    context.UserTripInstances.Where(uti => uti.TripId == TripId && uti.UserId == UserId).ToList();
                }
            }
            // TODO: eventually delete old trip instances (how old?)
            int total = Instances.Count(uti => uti.Attending == true && uti.CommuteMethod != CommuteMethod.HaveRide);
            int driveCount = Instances.Count(uti => uti.Attending == true && uti.CommuteMethod == CommuteMethod.Driver);
            return (double)driveCount / (double)total;
        }
    }
}