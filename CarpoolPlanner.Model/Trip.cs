using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.TimeZones;

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

        [Key]
        public long Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Location { get; set; }

        public TripStatus Status { get; set; }

        public string TimeZone { get; set; }

        [JsonIgnore]
        public DateTimeZone DateTimeZone
        {
            get { return TimeZone != null ? TzdbDateTimeZoneSource.Default.ForId(TimeZone) : null; }
        }

        public IList<TripRecurrence> Recurrences { get; set; }

        public IList<TripInstance> Instances { get; set; }

        public UserTripCollection UserTrips { get; set; }
    }
}