using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace CarpoolPlanner.Model
{
    public enum RecurrenceType
    {
        Yearly,
        YearlyByDayOfWeek,
        Monthly,
        MonthlyByDayOfWeek,
        Weekly,
        Daily
    }

    public class TripRecurrence
    {
        public TripRecurrence()
        {
            Type = RecurrenceType.Weekly;
            Every = 1;
            UserTripRecurrences = new UserTripRecurrenceCollection();
        }

        [Key]
        public long Id { get; set; }

        public long TripId { get; set; }

        public int Every { get; set; }

        [Required]
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public RecurrenceType Type { get; set; }

        public UserTripRecurrenceCollection UserTripRecurrences { get; set; }
        
        /// <summary>
        /// Gets the date of the next trip instance that occurs after the specified date.
        /// </summary>
        /// <param name="afterDate">A DateTime.</param>
        /// <returns>A DateTime.</returns>
        public DateTime? GetNextInstanceDate(DateTime afterDate)
        {
            if (Start == null)
                return null;
            DateTime date;
            switch (Type)
            {
                case RecurrenceType.Daily:
                    date = Start.Value.AddDays(Math.Ceiling((afterDate - Start.Value).TotalDays));
                    break;
                case RecurrenceType.Weekly:
                    date = Start.Value.AddDays(Math.Ceiling((afterDate - Start.Value).TotalDays / 7.0) * 7.0);
                    var date2 = date.ToLocalTime();
                    var date3 = Start.Value.ToLocalTime().AddDays(Math.Ceiling((afterDate - Start.Value).TotalDays / 7.0) * 7.0);
                    break;
                case RecurrenceType.Monthly:
                    date = new DateTime(afterDate.Year, afterDate.Month, Start.Value.Day);
                    if (date < afterDate)
                        date = date.AddMonths(1);
                    break;
                case RecurrenceType.Yearly:
                    date = new DateTime(afterDate.Year, Start.Value.Month, Start.Value.Day);
                    if (date < afterDate)
                        date = date.AddMonths(1);
                    break;
                case RecurrenceType.MonthlyByDayOfWeek:
                case RecurrenceType.YearlyByDayOfWeek:
                    date = Start.Value;
                    while (date < afterDate && (!End.HasValue || date <= End.Value))
                    {
                        var week = (date.Day - 1) / 7;
                        var dayOfWeek = (int)date.DayOfWeek;
                        var firstOfMonth = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day + 1);
                        firstOfMonth = firstOfMonth.AddMonths((Type == RecurrenceType.MonthlyByDayOfWeek ? Every : 12 * Every) - 1);
                        var firstOfMonthDayOfWeek = (int)firstOfMonth.DayOfWeek;
                        var diff = dayOfWeek - firstOfMonthDayOfWeek;
                        if (diff < 0)
                            diff += 7;
                        int dayOfMonth = 7 * week + diff;
                        // Note that dayOfMonth could be larger than the number of days in the month, pushing the date to the next month.
                        // This will cause weird things, so hopefully nobody does it :P
                        date = firstOfMonth.AddDays(dayOfMonth); // Note that dayOfMonth is 0-based, not 1-based like DateTime.Day
                        break;
                    }
                    break;
                default:
                    return null;
            }
            if (End.HasValue && date > End.Value)
                return null;
            return date;
        }
    }
}