using System;
using Xunit;

namespace Scheduler.TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void ExceptionNoCurrentDate()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Once;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);

            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia", ex.Message);
        }
        [Fact]
        public void ExceptionNoStartDate()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Once;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);

            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("Debe indicar la fecha limite de inicio.", ex.Message);
        }

        [Fact]
        public void ExceptionNoPeriodicityDailyWeekly()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Daily;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 0;

            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("La Periodicidad debe ser superior a 0.", ex.Message);

            sm.Type = RecurrenceTypesEnum.Weekly;
            ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("La Periodicidad debe ser superior a 0.", ex.Message);


        }

        [Fact]
        public void ExceptionCurrentDateBelowStartDate()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Once;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 1;


            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("La fecha de calculo es inferior a la fecha de inicio establecida", ex.Message);
        }
        [Fact]
        public void ExceptionEndHourMinorStartHour()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Daily;
            sm.Periodicity = 1;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyStartAt = new DateTime(2021, 1, 1, 10, 0, 0);
            sm.HourlyEndAt  = new DateTime(2021, 1, 1, 8, 0, 0);



            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("La hora 'desde' debe ser inferior a la hora 'hasta'.", ex.Message);
        }
        [Fact]
        public void ExceptionLimitEndDateBelowNextDate()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Daily;
            sm.Periodicity = 1;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.EndDate = new DateTime(2021, 1, 1, 0, 0, 0);


            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("La ocurrencia calculada supera la fecha limite establecida.", ex.Message);
        }
        [Fact]
        public void ExceptionWeeklyWithoutAnyWeekDay()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.Periodicity = 1;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);


            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.", ex.Message);
        }

        [Fact]
        public void ExceptionHourlyWithoutHourRange()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Daily;
            sm.Periodicity = 1;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;


            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("Debe indicarse las horas limite cuando el modo horario es periodico.", ex.Message);
        }
        [Fact]
        public void ExceptionHourlyWithoutEveryHours()
        {
            SchedulerManager sm = new SchedulerManager();

            sm.Type = RecurrenceTypesEnum.Daily;
            sm.Periodicity = 1;
            sm.CurrentDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyStartAt = new DateTime(1000, 1, 1, 4, 0, 0);
            sm.HourlyEndAt  = new DateTime(1000, 1, 1, 8, 0, 0);


            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());

            Assert.Equal("Debe indicarse un valor para la periodicidad horaria.", ex.Message);
        }



        [Fact]
        public void TypeOnce()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Once;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);

            sm.CurrentDate = new DateTime(2021, 1, 1, 13, 14, 15);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 1, 13, 14, 15));

            Assert.Equal(data.Description, "Occurs One. Schedule will be used on 01/01/2021 at 13:14 starting on 01/01/2021");


        }
        [Fact]
        public void TypeDaily_NoHourly()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Daily;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1,3, 0, 0, 0));

            Assert.Equal(data.Description, "Occurs every 2 day(s). Schedule will be used on 03/01/2020 at 00:00 starting on 01/01/2020");


        }
        [Fact]
        public void TypeDaily_HourlyFixed()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Daily;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursOne;
            sm.HourlyOccursAt = new DateTime(2000, 1, 1, 12, 13, 14);

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 12, 13, 14));

            Assert.Equal(data.Description, "Occurs every 2 day(s) at 12:13. Schedule will be used on 03/01/2020 at 12:13 starting on 01/01/2020");
        }
        [Fact]
        public void TypeDaily_HourlyEvery()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Daily;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyOccursEvery  = 2;
            sm.HourlyStartAt = new DateTime(2000, 1, 1, 4, 0, 0);
            sm.HourlyEndAt  = new DateTime(2000, 1, 1, 8, 0, 0);

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/01/2020 at 04:00 starting on 01/01/2020");


            sm.CurrentDate = new DateTime(2020, 1, 1, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 1, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/01/2020 at 08:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 1, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 3, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 06:00 starting on 01/01/2020");

            // Comprueba que funciona correctamente con el limite hasta puesto en las 23 horas

            sm.HourlyStartAt = new DateTime(2000, 1, 1, 0, 0, 0);
            sm.HourlyEndAt = new DateTime(2000, 1, 1, 23, 0, 0);
            sm.CurrentDate = new DateTime(2020, 1, 1, 22, 0, 0);

            sm.CurrentDate = new DateTime(2020, 1, 1, 22, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 00:00 and 23:00. Schedule will be used on 03/01/2020 at 00:00 starting on 01/01/2020");



        }
        [Fact]
        public void TypeDaily_WeeklyNoHourly()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday); 
            sm.Periodicity = 2;

            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Schedule will be used on 02/01/2020 at 00:00 starting on 01/01/2020");
            

            sm.CurrentDate = new DateTime(2020, 1, 2, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Schedule will be used on 03/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 3, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Schedule will be used on 13/01/2020 at 00:00 starting on 01/01/2020");


            sm.CurrentDate = new DateTime(2020, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Schedule will be used on 16/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Schedule will be used on 17/01/2020 at 00:00 starting on 01/01/2020");

        }

        [Fact]
        public void TypeDaily_WeeklyHourlyFixed()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday);
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursOne;
            sm.HourlyOccursAt = new DateTime(2000, 1, 1, 12, 13, 14);
            sm.Periodicity = 2;

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday at 12:13. Schedule will be used on 02/01/2020 at 12:13 starting on 01/01/2020");
            

            sm.CurrentDate = new DateTime(2020, 1, 2, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday at 12:13. Schedule will be used on 03/01/2020 at 12:13 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 3, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday at 12:13. Schedule will be used on 13/01/2020 at 12:13 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday at 12:13. Schedule will be used on 16/01/2020 at 12:13 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday at 12:13. Schedule will be used on 17/01/2020 at 12:13 starting on 01/01/2020");
        }
        [Fact]
        public void TypeDaily_WeeklyHourlyEvery()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyOccursEvery = 2;
            sm.HourlyStartAt = new DateTime(2000, 1, 1, 4, 0, 0);
            sm.HourlyEndAt = new DateTime(2000, 1, 1, 8, 0, 0);

            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 02/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 2, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 02/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 2, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 02/01/2020 at 08:00 starting on 01/01/2020");

            // SALTA DE DIA

            sm.CurrentDate = new DateTime(2020, 1, 2, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 3, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 3, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 08:00 starting on 01/01/2020");

            // SUMA 2 SEMANAS
            sm.CurrentDate = new DateTime(2020, 1, 3, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/01/2020 at 08:00 starting on 01/01/2020");

            // CAMBIO DE DIA
            sm.CurrentDate = new DateTime(2020, 1, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 16/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 16, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 16/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 16, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 16/01/2020 at 08:00 starting on 01/01/2020");
            // CAMBIO DE DIA
            sm.CurrentDate = new DateTime(2020, 1, 16, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 17/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 17, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 17/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 17, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on Monday, Thursday, Friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 17/01/2020 at 08:00 starting on 01/01/2020");


        }
        [Fact]
        public void TypeDaily_WeeklyNoHourlyAllWeekDays()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Tuesday | WeekDaysEnum.Wednesday | WeekDaysEnum.Thursday | 
                            WeekDaysEnum.Friday | WeekDaysEnum.Saturday | WeekDaysEnum.Sunday);
            sm.Periodicity = 1;

            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 2, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 02/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 2, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 03/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 3, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 04/01/2020 at 00:00 starting on 01/01/2020");


            sm.CurrentDate = new DateTime(2020, 1, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 05/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 5, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 6, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 06/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 6, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 7, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 07/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 7, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 8, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 08/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 8, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 9, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 09/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 9, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 10, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 10/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 10, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 11/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 12/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 12, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 13/01/2020 at 00:00 starting on 01/01/2020");


            sm.CurrentDate = new DateTime(2020, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 14/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 14, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 15, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 15/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 15, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 16, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 16/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 17, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 17/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 18/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 19/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 19, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday. Schedule will be used on 20/01/2020 at 00:00 starting on 01/01/2020");


        }

    }
}
