using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

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
        }

        [Key]
        public long Id { get; set; }

        public long TripId { get; set; }

        public int Every { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public RecurrenceType Type { get; set; }
        
        /// <summary>
        /// Gets the date of the next trip instance that occurs after the current date.
        /// </summary>
        /// <returns>A DateTime.</returns>
        public DateTime GetNextInstanceDate()
        {
            return GetNextInstanceDate(DateTime.Now);
        }

        /// <summary>
        /// Gets the date of the next trip instance that occurs after the specified date.
        /// </summary>
        /// <param name="afterDate">A DateTime.</param>
        /// <returns>A DateTime.</returns>
        public DateTime GetNextInstanceDate(DateTime afterDate)
        {
            if (Start == null)
                return default(DateTime);
            var date = Start.Value;
            while (date < afterDate)
            {
                switch (Type)
                {
                    case RecurrenceType.Daily:
                        date = date.AddDays(Every);
                        break;
                    case RecurrenceType.Weekly:
                        date = date.AddDays(Every * 7);
                        break;
                    case RecurrenceType.Monthly:
                        date = date.AddMonths(Every);
                        break;
                    case RecurrenceType.Yearly:
                        date = date.AddYears(Every);
                        break;
                    case RecurrenceType.MonthlyByDayOfWeek:
                    case RecurrenceType.YearlyByDayOfWeek:
                        var week = (date.Day - 1) / 7;
                        var dayOfWeek = (int)date.DayOfWeek;
                        var firstOfMonth = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - date.Day + 1);
                        firstOfMonth = firstOfMonth.AddMonths((Type == RecurrenceType.MonthlyByDayOfWeek ? Every : 12 * Every) - 1);
                        var firstOfMonthDayOfWeek = (int)firstOfMonth.DayOfWeek;
                        var diff = dayOfWeek - firstOfMonthDayOfWeek;
                        if (diff < 0)
                            diff += 7;
                        int dayOfMonth = 7 * week + diff;
                        // TODO: test
                        // Note that dayOfMonth could be larger than the number of days in the month, pushing the date to the next month.
                        // This will cause weird things, so hopefully nobody does it :P
                        date = firstOfMonth.AddDays(dayOfMonth); // Note that dayOfMonth is 0-based, not 1-based like DateTime.Day
                        date = date.AddMonths(Every);
                        break;
                }
            }
            return date;
        }

        public string DisplayValue
        {
            get
            {
                if (Start == null)
                    return string.Empty;
                var sb = new StringBuilder();
                switch (Type)
                {
                    case RecurrenceType.Yearly:
                        sb.Append(Start.Value.ToLongDateString());
                        sb.Append(" every ");
                        if (Every == 1)
                        {
                            sb.Append("year");
                        }
                        else
                        {
                            sb.Append(Every);
                            sb.Append(" years");
                        }
                        break;
                    case RecurrenceType.YearlyByDayOfWeek:
                    case RecurrenceType.MonthlyByDayOfWeek:
                        // TODO: test
                        sb.Append("The ");
                        int week = (Start.Value.Day - 1) / 7 + 1;
                        sb.Append(week);
                        sb.Append(GetSuffix(week));
                        sb.AppendFormat(" {0:dddd} of", Start);
                        if (Type == RecurrenceType.YearlyByDayOfWeek)
                            sb.AppendFormat(" {0:MMMM}", Start);
                        sb.Append(" every ");
                        if (Every == 1)
                        {
                            sb.Append(Type == RecurrenceType.YearlyByDayOfWeek ? "year" : "month");
                        }
                        else
                        {
                            sb.Append(Every);
                            sb.Append(Type == RecurrenceType.YearlyByDayOfWeek ? " years" : " months");
                        }
                        break;
                    case RecurrenceType.Monthly:
                        sb.Append("The ");
                        sb.Append(Start.Value.Day);
                        sb.Append(GetSuffix(Start.Value.Day));
                        sb.Append(" of every ");
                        if (Every == 1)
                        {
                            sb.Append("month");
                        }
                        else
                        {
                            sb.Append(Every);
                            sb.Append(" months");
                        }
                        break;
                    case RecurrenceType.Weekly:
                        sb.Append("Every ");
                        if (Every > 1)
                        {
                            sb.Append(Every);
                            sb.Append(GetSuffix(Every));
                            sb.Append(" ");
                        }
                        sb.Append(Start.Value.ToString("dddd"));
                        break;
                    case RecurrenceType.Daily:
                        sb.Append("Every day ");
                        break;
                }
                sb.AppendFormat(" at {0:h:mm:ss tt}", Start);
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return DisplayValue;
        }

        private string GetSuffix(int num)
        {
            if (num < 0)
                num *= -1;
            num %= 100;
            if (num >= 11 && num <= 13)
                return "th";
            switch (num % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}