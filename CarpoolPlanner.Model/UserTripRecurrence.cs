using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.Model
{
    public class UserTripRecurrence
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public long TripRecurrenceId { get; set; }

        public long TripId { get; set; }

        [Association("fk_usertriprecurrence_user", "user_id", "id")]
        public User User { get; set; }

        [Association("fk_usertriprecurrence_triprecurrence", "trip_recurrence_id", "id")]
        public TripRecurrence TripRecurrence { get; set; }

    }
}