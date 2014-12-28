using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.Model
{
    public enum TripStatus
    {
        Active,
        Disabled
    }

    public class Trip
    {
        public Trip()
        {
            Recurrences = new List<TripRecurrence>();
            Instances = new List<TripInstance>();
            UserTrips = new UserTripCollection();
        }

        // TODO: use JsonPropertyAttribute to make name lowercase in javascript
        [Key]
        public long Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Location { get; set; }

        public TripStatus Status { get; set; }

        public IList<TripRecurrence> Recurrences { get; private set; }

        public IList<TripInstance> Instances { get; private set; }

        public UserTripCollection UserTrips { get; private set; }
    }
}