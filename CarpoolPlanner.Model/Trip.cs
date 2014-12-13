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
        }

        [Key]
        public long Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Location { get; set; }

        public TripStatus Status { get; set; }

        public ICollection<TripRecurrence> Recurrences { get; private set; }

        public ICollection<TripInstance> Instances { get; private set; }
    }
}