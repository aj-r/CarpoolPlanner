using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CarpoolPlanner.Model
{
    public class UserTripRecurrence
    {
        [Key, Column(Order = 0)]
        public long UserId { get; set; }

        [Key, Column(Order = 1)]
        public long TripRecurrenceId { get; set; }

        public long TripId { get; set; }

        public bool Attending { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("TripRecurrenceId")]
        [JsonIgnore]
        public TripRecurrence TripRecurrence { get; set; }

    }
}