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
            LocalDateTime dateTime;
            switch (Type)
            {
                case RecurrenceType.Daily:
                    dateTime = localAfterDateTime.Date.At(localStartDateTime.TimeOfDay);
                    if (dateTime < localAfterDateTime)
                        dateTime += Period.FromDays(1);
                    break;
                case RecurrenceType.Weekly:
                    dateTime = localAfterDateTime.Date.At(localStartDateTime.TimeOfDay);
                    if (dateTime.DayOfWeek != localStartDateTime.DayOfWeek || dateTime < localAfterDateTime)
                        dateTime = dateTime.Next((IsoDayOfWeek)localStartDateTime.Date.DayOfWeek);
                    break;
                case RecurrenceType.Monthly:
                {
                    int day = localStartDateTime.Day;
                    var daysInMonth = localAfterDateTime.Calendar.GetDaysInMonth(localAfterDateTime.Year, localAfterDateTime.Month);
                    if (day > daysInMonth)
                        day = 1;
                    dateTime = new LocalDate(localAfterDateTime.Year, localAfterDateTime.Month, day).At(localStartDateTime.TimeOfDay);
                    if (dateTime < localAfterDateTime)
                        dateTime = dateTime.PlusMonths(1);

                    daysInMonth = dateTime.Calendar.GetDaysInMonth(dateTime.Year, dateTime.Month);
                    if (day > daysInMonth)
                        dateTime = new LocalDate(dateTime.Year, dateTime.Month, 1).PlusMonths(1).At(localStartDateTime.TimeOfDay);

                    break;
                }
                case RecurrenceType.Yearly:
                {
                    int day = localStartDateTime.Day;
                    int month = localStartDateTime.Month;
                    var daysInMonth = localAfterDateTime.Calendar.GetDaysInMonth(localAfterDateTime.Year, month);
                    if (day > daysInMonth)
                    {
                        day = 1;
                        month = (month < 12 ? month + 1 : 1);
                    }
                    dateTime = new LocalDate(localAfterDateTime.Year, month, day).At(localStartDateTime.TimeOfDay);
                    if (dateTime < localAfterDateTime)
                        dateTime = dateTime.PlusYears(1);
                    break;
                }
                case RecurrenceType.MonthlyByDayOfWeek:
                case RecurrenceType.YearlyByDayOfWeek:
                {
                    var zeroBasedWeek = (localStartDateTime.Day - 1) / 7;
                    var dayOfWeek = (int)localStartDateTime.DayOfWeek;

                    var month = Type == RecurrenceType.MonthlyByDayOfWeek ? localAfterDateTime.Month : localStartDateTime.Month;
                    var year = localAfterDateTime.Year;
                    var firstOfMonth = new LocalDate(year, month, 1);

                    do
                    {
                        var diff = dayOfWeek - firstOfMonth.DayOfWeek;
                        if (diff < 0)
                            diff += 7;
                        dateTime = firstOfMonth.PlusDays(7 * zeroBasedWeek + diff).At(localStartDateTime.TimeOfDay);
                        if (Type == RecurrenceType.MonthlyByDayOfWeek)
                            firstOfMonth = firstOfMonth.PlusMonths(1);
                        else
                            firstOfMonth = firstOfMonth.PlusYears(1);

                    } while (dateTime < localAfterDateTime);
                    break;
                }
                default:
                    return null;
            }
            var dateTimeUtc = dateTime.InZoneLeniently(tripTimeZone).ToDateTimeUtc();
            if (End != null && dateTimeUtc > End.Value)
                return null;
            return dateTimeUtc;
        }
    }
}