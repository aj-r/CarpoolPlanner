using System;
using CarpoolPlanner.Model;
using NodaTime;
using NodaTime.TimeZones;
using NUnit.Framework;

namespace CarpoolPlanner.Tests.Model
{
    [TestFixture]
    public class UserTripRecurrenceTests
    {
        [Test]
        public void NextInstanceDateTest_Daily()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 2, 23, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 23, 12, 0, 1, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 2, 24, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Daily_TimeZone()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 2, 23, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 23, 12, 0, 1, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 2, 24, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Weekly()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 1, 18, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Weekly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 1, 28, 12, 0, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 2, 1, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Weekly_TimeZone()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 2, 23, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Weekly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 23, 12, 0, 1, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 3, 2, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Monthly()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 1, 18, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Monthly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 1, 28, 12, 0, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 2, 18, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Monthly_NonExistentDate1()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2014, 12, 31, 14, 45, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Monthly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 15, 14, 45, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            // Non-existent day of month. It should take the first day of the next month.
            Assert.AreEqual(new DateTime(2015, 3, 1, 14, 45, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Monthly_NonExistentDate2()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2014, 12, 31, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Monthly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 1, 31, 12, 0, 1, DateTimeKind.Utc),
                DateTimeZone.Utc);
            // Non-existent day of month. It should take the first day of the next month.
            Assert.AreEqual(new DateTime(2015, 3, 1, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Monthly_TimeZone()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 1, 23, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Monthly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 3, 12, 0, 1, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 2, 23, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Yearly1()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2010, 1, 31, 23, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Yearly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2014, 2, 23, 12, 0, 1, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 1, 31, 23, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Yearly2()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2010, 1, 31, 23, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Yearly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2014, 1, 31, 23, 0, 1, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 1, 31, 23, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Yearly_NonExistentDate()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2016, 2, 29, 11, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Yearly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2017, 1, 15, 14, 45, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            // Non-existent day of month. It should take the first day of the next month.
            Assert.AreEqual(new DateTime(2017, 3, 1, 11, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Yearly_TimeZone()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2010, 1, 31, 23, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Yearly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2014, 2, 23, 12, 0, 1, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 1, 31, 23, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_MonthlyByDayOfWeek1()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 15, 13, 0, 0, DateTimeKind.Utc), // 3rd Sunday
                Every = 1,
                Type = RecurrenceType.MonthlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 6, 16, 13, 1, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 6, 21, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_MonthlyByDayOfWeek2()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 15, 13, 0, 0, DateTimeKind.Utc), // 3rd Sunday
                Every = 1,
                Type = RecurrenceType.MonthlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 6, 21, 13, 1, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 7, 19, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_MonthlyByDayOfWeek3()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 1, 29, 13, 0, 0, DateTimeKind.Utc), // 5th Thursday
                Every = 1,
                Type = RecurrenceType.MonthlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 4, 2, 13, 1, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 4, 30, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_MonthlyByDayOfWeek_NonExistent()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 31, 13, 0, 0, DateTimeKind.Utc), // 5th Tuesday
                Every = 1,
                Type = RecurrenceType.MonthlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 5, 16, 13, 1, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            // Non-existent day. Do the 1st Tuesday of next month.
            Assert.AreEqual(new DateTime(2015, 6, 2, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_MonthlyByDayOfWeek_TimeZone()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 15, 13, 0, 0, DateTimeKind.Utc), // 3rd Sunday
                Every = 1,
                Type = RecurrenceType.MonthlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 6, 16, 13, 1, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 6, 21, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_YearlyByDayOfWeek()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 15, 13, 0, 0, DateTimeKind.Utc), // 3rd Sunday
                Every = 1,
                Type = RecurrenceType.YearlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 6, 16, 13, 1, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2016, 3, 20, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_YearlyByDayOfWeek_NonExistent()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2016, 2, 29, 13, 0, 0, DateTimeKind.Utc), // 5th Monday
                Every = 1,
                Type = RecurrenceType.YearlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2016, 3, 21, 13, 1, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            // Non-existent day. Do the 1st Monday of next month.
            Assert.AreEqual(new DateTime(2017, 3, 6, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_YearlyByDayOfWeek_TimeZone()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 15, 13, 0, 0, DateTimeKind.Utc), // 3rd Sunday
                Every = 1,
                Type = RecurrenceType.YearlyByDayOfWeek
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 6, 16, 13, 1, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2016, 3, 20, 13, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_DaylightSavings_Forward1()
        {
            // Daylight savings on March 8, 2015 at 2:00AM (7:00AM UTC)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 1, 7, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 3, 9, 5, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            // Should be at 6:00 UTC (even though the start time is 7:00 UTC in the TripRecurrence definition)
            // to account for the daylight savings time change.
            Assert.AreEqual(new DateTime(2015, 3, 9, 6, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_DaylightSavings_Forward2()
        {
            // Daylight savings on March 8, 2015 at 2:00AM (7:00AM UTC)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 1, 7, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 3, 8, 7, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 3, 9, 6, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_DaylightSavings_Forward3()
        {
            // Daylight savings on March 8, 2015 at 2:00AM (7:00AM UTC)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 3, 7, 13, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 3, 8, 7, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_DaylightSavings_Forward4()
        {
            // Daylight savings on March 8, 2015 at 2:00AM (7:00AM UTC)
            // Non-existent time (2:30 AM) - should pick the next available time (3:00 AM)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 1, 7, 30, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 3, 7, 13, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 3, 8, 7, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_DaylightSavings_Forward5()
        {
            // Daylight savings on March 8, 2015 at 2:00AM (8:00AM UTC)
            // Test with a different time zone
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 3, 1, 9, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 3, 7, 13, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("Canada/Central"));
            Assert.AreEqual(new DateTime(2015, 3, 8, 8, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Daily_DaylightSavings4()
        {
            // Daylight savings on November 1, 2015 at 2:00AM (6:00AM UTC)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 6, 27, 6, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 11, 1, 8, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 11, 2, 7, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Daily_DaylightSavings6()
        {
            // Daylight savings on November 1, 2015 at 2:00AM (6:00AM UTC)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 6, 27, 7, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 11, 1, 7, 30, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 11, 1, 8, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Daily_DaylightSavings7()
        {
            // Daylight savings on November 1, 2015 at 2:00AM (6:00AM UTC)
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 6, 27, 7, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 11, 1, 4, 0, 0, DateTimeKind.Utc),
                TzdbDateTimeZoneSource.Default.ForId("America/New_York"));
            Assert.AreEqual(new DateTime(2015, 11, 1, 8, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_BeforeStart()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 2, 23, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 1, 20, 12, 0, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 2, 23, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_LastInstance()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 1, 23, 12, 0, 0, DateTimeKind.Utc),
                End = new DateTime(2015, 2, 15, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Daily
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 15, 12, 0, 0, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.AreEqual(new DateTime(2015, 2, 15, 12, 0, 0, DateTimeKind.Utc), date);
        }

        [Test]
        public void NextInstanceDateTest_Ended()
        {
            var tr = new TripRecurrence
            {
                Start = new DateTime(2015, 1, 23, 12, 0, 0, DateTimeKind.Utc),
                End = new DateTime(2015, 2, 15, 12, 0, 0, DateTimeKind.Utc),
                Every = 1,
                Type = RecurrenceType.Weekly
            };
            var date = tr.GetNextInstanceDate(new DateTime(2015, 2, 15, 12, 0, 1, DateTimeKind.Utc),
                DateTimeZone.Utc);
            Assert.IsNull(date);
        }
    }
}
