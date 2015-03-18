using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using NodaTime;

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
        public DateTime? GetNextInstanceDate(DateTime afterDate, DateTimeZone tripTimeZone)
        {
            if (Start == null)
                return null;
            if (End != null && afterDate > End.Value)
                return null;
            if (afterDate < Start.Value)
                return Start.Value;
            var localStartDateTime = Instant.FromDateTimeUtc(Start.Value).InZone(tripTimeZone).LocalDateTime;
            var localAfterDateTime = Instant.FromDateTimeUtc(afterDate).InZone(tripTimeZone).LocalDateTime;
            LocalDate date;
            switch (Type)
            {
                case RecurrenceType.Daily:
                    date = localAfterDateTime.Date;
                    if (localStartDateTime.TimeOfDay < localAfterDateTime.TimeOfDay)
                        date += Period.FromDays(1);
                    break;
                case RecurrenceType.Weekly:
                    date = localAfterDateTime.Date;
                    if (date.DayOfWeek != localStartDateTime.DayOfWeek || localStartDateTime.TimeOfDay < localAfterDateTime.TimeOfDay)
                        date = date.Next((IsoDayOfWeek)localStartDateTime.Date.DayOfWeek);
                    break;
                case RecurrenceType.Monthly:
                {
                    int day = localStartDateTime.Day;
                    var daysInMonth = localAfterDateTime.Calendar.GetDaysInMonth(localAfterDateTime.Year, localAfterDateTime.Month);
                    if (day > daysInMonth)
                        day = 1;
                    date = new LocalDate(localAfterDateTime.Year, localAfterDateTime.Month, day);
                    if (date < localAfterDateTime.Date || (date == localAfterDateTime.Date && localStartDateTime.TimeOfDay < localAfterDateTime.TimeOfDay))
                        date = date.PlusMonths(1);
                    break;
                }
                case RecurrenceType.Yearly:
                {
                    int day = localStartDateTime.Day;
                    var daysInMonth = localAfterDateTime.Calendar.GetDaysInMonth(localAfterDateTime.Year, localAfterDateTime.Month);
                    if (day > daysInMonth)
                        day = 1;
                    date = new LocalDate(localAfterDateTime.Year, localStartDateTime.Month, day);
                    if (date < localAfterDateTime.Date || (date == localAfterDateTime.Date && localStartDateTime.TimeOfDay < localAfterDateTime.TimeOfDay))
                        date = date.PlusYears(1);
                    break;
                }
                case RecurrenceType.MonthlyByDayOfWeek:
                case RecurrenceType.YearlyByDayOfWeek:
                    // TODO: convert to noda time
                    date = localStartDateTime.Date;
                    while (date < localAfterDateTime.Date)
                    {
                        var week = (date.WeekOfWeekYear - 1) / 7;// TODO: this is wrong
                        var dayOfWeek = (int)date.DayOfWeek;
                        var firstOfMonth = date.PlusDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day + 1);
                        firstOfMonth = firstOfMonth.PlusMonths((Type == RecurrenceType.MonthlyByDayOfWeek ? Every : 12 * Every) - 1);
                        var firstOfMonthDayOfWeek = (int)firstOfMonth.DayOfWeek;
                        var diff = dayOfWeek - firstOfMonthDayOfWeek;
                        if (diff < 0)
                            diff += 7;
                        int dayOfMonth = 7 * week + diff;
                        // Note that dayOfMonth could be larger than the number of days in the month, pushing the date to the next month.
                        // This will cause weird things, so hopefully nobody does it :P
                        date = firstOfMonth.PlusMonths(dayOfMonth); // Note that dayOfMonth is 0-based, not 1-based like DateTime.Day
                        break;
                    }
                    break;
                default:
                    return null;
            }
            var dateTime = date.At(localStartDateTime.TimeOfDay).InUtc().ToDateTimeUtc();
            if (End != null && dateTime > End.Value)
                return null;
            return dateTime;
        }
    }
}