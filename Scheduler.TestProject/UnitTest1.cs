using System;
using Xunit;

namespace Scheduler.TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void TypeOnce()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Once;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);

            sm.CurrentDate = new DateTime(2021, 1, 1, 13, 14, 15);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 1, 13, 14, 15));

        }
        [Fact]
        public void TypeDaily_NoHourly()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Daily;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1,3, 0, 0, 0));
        }
        [Fact]
        public void TypeDaily_HourlyFixed()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Daily;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencys.OccursOne;
            sm.HourlyOccursAt = new DateTime(2000, 1, 1, 12, 13, 14);

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 12, 13, 14));

        }
        [Fact]
        public void TypeDaily_HourlyEvery()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Daily;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencys.OccursEvery;
            sm.HourlyOccursEvery  = 2;
            sm.HourlyStartAt = new DateTime(2000, 1, 1, 4, 0, 0);
            sm.HourlyEndAt  = new DateTime(2000, 1, 1, 8, 0, 0);

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 4, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 1, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 6, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 1, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 8, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 1, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 4, 0, 0));


            sm.CurrentDate = new DateTime(2020, 1, 3, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 6, 0, 0));
        }
        [Fact]
        public void TypeDaily_WeeklyNoHourly()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday); 
            sm.Periodicity = 2;

            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 0, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 2, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 0, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 3, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 0, 0, 0));


            sm.CurrentDate = new DateTime(2020, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 0, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 0, 0, 0));

        }

        [Fact]
        public void TypeDaily_WeeklyHourlyFixed()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday);
            sm.HourlyFrecuency = HourlyFrecuencys.OccursOne;
            sm.HourlyOccursAt = new DateTime(2000, 1, 1, 12, 13, 14);
            sm.Periodicity = 2;

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 12, 13, 14));

            sm.CurrentDate = new DateTime(2020, 1, 2, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 12, 13, 14));

            sm.CurrentDate = new DateTime(2020, 1, 3, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 12, 13, 14));

            sm.CurrentDate = new DateTime(2020, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 12, 13, 14));

            sm.CurrentDate = new DateTime(2020, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 12, 13, 14));
        }
        [Fact]
        public void TypeDaily_WeeklyHourlyEvery()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypes.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencys.OccursEvery;
            sm.HourlyOccursEvery = 2;
            sm.HourlyStartAt = new DateTime(2000, 1, 1, 4, 0, 0);
            sm.HourlyEndAt = new DateTime(2000, 1, 1, 8, 0, 0);

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 4, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 2, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 6, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 2, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 8, 0, 0));

            // SALTA DE DIA

            sm.CurrentDate = new DateTime(2020, 1, 2, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 4, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 3, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 6, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 3, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 8, 0, 0));

            // SUMA 2 SEMANAS
            sm.CurrentDate = new DateTime(2020, 1, 3, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 4, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 6, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 8, 0, 0));

            // CAMBIO DE DIA
            sm.CurrentDate = new DateTime(2020, 1, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 4, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 16, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 6, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 16, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 8, 0, 0));
            // CAMBIO DE DIA
            sm.CurrentDate = new DateTime(2020, 1, 16, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 4, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 17, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 6, 0, 0));

            sm.CurrentDate = new DateTime(2020, 1, 17, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 8, 0, 0));


        }

    }
}
