using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NodaTime;
using NodaTime.TimeZones;

namespace CarpoolPlanner.ViewModel
{
    public class TimeZoneViewModel
    {
        public static TimeZoneViewModelCollection GetAll()
        {
            var zones = TzdbDateTimeZoneSource.Default.GetIds();
            var vms = zones.Select(t => new TimeZoneViewModel(t));
            return new TimeZoneViewModelCollection(vms);
        }

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="name">The tz database name of the time zone.</param>
        public TimeZoneViewModel(string name)
        {
            Name = name;
            Offset = TzdbDateTimeZoneSource.Default.ForId(name).GetUtcOffset(SystemClock.Instance.Now).ToString();
        }

        /// <summary>
        /// Gets the tz database name of the time zone.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the current UTC offset for the time zone.
        /// </summary>
        public string Offset { get; private set; }

        public override string ToString()
        {
            return Name + " (UTC" + Offset + ")";
        }
    }
}