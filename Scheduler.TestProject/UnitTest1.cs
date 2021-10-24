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
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 14, 15);

            sm.Language = SupportedLanguagesEnum.es_ES;
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 13, 14, 15));
            Assert.Equal(data.Description, "Ocurre una sola vez. La siguiente cita se producirá el 13/01/2021 a las 13:14 comenzando el 01/01/2021");

            sm.Language = SupportedLanguagesEnum.en_GB;
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 13, 14, 15));
            Assert.Equal(data.Description, "Occurs One. Schedule will be used on 13/01/2021 at 13:14 starting on 01/01/2021");

            sm.Language = SupportedLanguagesEnum.en_US;
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 13, 14, 15));
            Assert.Equal(data.Description, "Occurs One. Schedule will be used on 1/13/2021 at 1:14 PM starting on 1/1/2021");

        }
        [Fact]
        public void TypeDaily_NoHourly()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Daily;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 14, 0);


            sm.Language = SupportedLanguagesEnum.es_ES;
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1,15, 13, 14, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s). La siguiente cita se producirá el 15/01/2021 a las 13:14 comenzando el 01/01/2021");

            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 15, 13, 14, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 17, 13, 14, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s). La siguiente cita se producirá el 17/01/2021 a las 13:14 comenzando el 01/01/2021");


            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 14, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 13, 14, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Schedule will be used on 15/01/2021 at 13:14 starting on 01/01/2021");

            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 15, 13, 14, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 17, 13, 14, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Schedule will be used on 17/01/2021 at 13:14 starting on 01/01/2021");


            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 14, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 13,14,0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Schedule will be used on 1/15/2021 at 1:14 PM starting on 1/1/2021");

            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 15, 13, 14, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 17, 13, 14, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Schedule will be used on 1/17/2021 at 1:14 PM starting on 1/1/2021");

        }
        [Fact]
        public void TypeDaily_HourlyFixed()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Daily;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursOne;
            sm.HourlyOccursAt = new DateTime(1001, 1, 1, 12, 13, 14);

            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 12, 13, 14));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s) a las 12:13. La siguiente cita se producirá el 13/01/2021 a las 12:13 comenzando el 01/01/2021");

            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 12, 13, 14));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s) a las 12:13. La siguiente cita se producirá el 15/01/2021 a las 12:13 comenzando el 01/01/2021");


            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 day(s) at 12:13. Schedule will be used on 13/01/2021 at 12:13 starting on 01/01/2021");

            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 day(s) at 12:13. Schedule will be used on 15/01/2021 at 12:13 starting on 01/01/2021");


            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 day(s) at 12:13. Schedule will be used on 1/13/2021 at 12:13 PM starting on 1/1/2021");

            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 day(s) at 12:13. Schedule will be used on 1/15/2021 at 12:13 PM starting on 1/1/2021");
        }
        [Fact]
        public void TypeDaily_HourlyEvery()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Daily;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyOccursEvery  = 2;
            sm.HourlyStartAt = new DateTime(2000, 1, 1, 4, 0, 0);
            sm.HourlyEndAt  = new DateTime(2000, 1, 1, 8, 0, 0);

            // SPANISH
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/01/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/01/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/01/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 dia(s). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 15/01/2021 a las 4:00 comenzando el 01/01/2021");

            // ENGLISH UK
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/01/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/01/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/01/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 15/01/2021 at 04:00 starting on 01/01/2021");

            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/13/2021 at 4:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/13/2021 at 6:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 13, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/13/2021 at 8:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/15/2021 at 4:00 AM starting on 1/1/2021");



            /*   sm.CurrentDate = new DateTime(2020, 1, 1, 4, 0, 0);
               data = sm.NextOcurrence();
               Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 6, 0, 0));
               Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/01/2020 at 6:00 starting on 01/01/2020");

               sm.CurrentDate = new DateTime(2020, 1, 1, 6, 0, 0);
               data = sm.NextOcurrence();
               Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 1, 8, 0, 0));
               Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/01/2020 at 8:00 starting on 01/01/2020");

               sm.CurrentDate = new DateTime(2020, 1, 1, 8, 0, 0);
               data = sm.NextOcurrence();
               Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 4, 0, 0));
               Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 4:00 starting on 01/01/2020");

               sm.CurrentDate = new DateTime(2020, 1, 3, 4, 0, 0);
               data = sm.NextOcurrence();
               Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 6, 0, 0));
               Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 03/01/2020 at 6:00 starting on 01/01/2020");

               // Comprueba que funciona correctamente con el limite hasta puesto en las 23 horas

               sm.HourlyStartAt = new DateTime(2000, 1, 1, 0, 0, 0);
               sm.HourlyEndAt = new DateTime(2000, 1, 1, 23, 0, 0);
               sm.CurrentDate = new DateTime(2020, 1, 1, 22, 0, 0);

               sm.CurrentDate = new DateTime(2020, 1, 1, 22, 0, 0);
               data = sm.NextOcurrence();
               Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 3, 0, 0, 0));
               Assert.Equal(data.Description, "Occurs every 2 day(s). Every 2 hour(s) between 00:00 and 23:00. Schedule will be used on 03/01/2020 at 0:00 starting on 01/01/2020");

               */

        }
        [Fact]
        public void TypeWeekly_NoHourly()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday); 
            sm.Periodicity = 2;

            // SPANISH
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 45, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 13, 45, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. La siguiente cita se producirá el 14/01/2021 a las 13:45 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 13, 45, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. La siguiente cita se producirá el 15/01/2021 a las 13:45 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 13, 45, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. La siguiente cita se producirá el 25/01/2021 a las 13:45 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 13, 45, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. La siguiente cita se producirá el 28/01/2021 a las 13:45 comenzando el 01/01/2021");

            // ENGLISH Uk
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 14/01/2021 at 13:45 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 15/01/2021 at 13:45 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 25/01/2021 at 13:45 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 28/01/2021 at 13:45 starting on 01/01/2021");


            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 1/14/2021 at 1:45 PM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 1/15/2021 at 1:45 PM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 1/25/2021 at 1:45 PM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 13, 45, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 13, 45, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Schedule will be used on 1/28/2021 at 1:45 PM starting on 1/1/2021");


        }

        [Fact]
        public void TypeWeekly_HourlyFixed()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday);
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursOne;
            sm.HourlyOccursAt = new DateTime(1001, 1, 1, 12, 13, 14);
            sm.Periodicity = 2;

            // SPANISH
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 12, 13, 14));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes a las 12:13. La siguiente cita se producirá el 14/01/2021 a las 12:13 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 12, 13, 14));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes a las 12:13. La siguiente cita se producirá el 15/01/2021 a las 12:13 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 12, 13, 14));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes a las 12:13. La siguiente cita se producirá el 25/01/2021 a las 12:13 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 12, 13, 14));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes a las 12:13. La siguiente cita se producirá el 28/01/2021 a las 12:13 comenzando el 01/01/2021");

            // ENGLISH UK
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 14/01/2021 at 12:13 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 15/01/2021 at 12:13 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 25/01/2021 at 12:13 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 28/01/2021 at 12:13 starting on 01/01/2021");

            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 1/14/2021 at 12:13 PM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 1/15/2021 at 12:13 PM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 1/25/2021 at 12:13 PM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 12, 13, 14);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 12, 13, 14));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday at 12:13. Schedule will be used on 1/28/2021 at 12:13 PM starting on 1/1/2021");
        }
        [Fact]
        public void TypeWeekly_HourlyEvery()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Thursday | WeekDaysEnum.Friday);
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyOccursEvery = 2;
            sm.HourlyStartAt = new DateTime(1001, 1, 1, 4, 0, 0);
            sm.HourlyEndAt = new DateTime(1001, 1, 1, 8, 0, 0);

            // SPANISH    
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 14/01/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 14/01/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 14/01/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 15/01/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 15/01/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 15/01/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/01/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/01/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/01/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 28/01/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 28, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 28/01/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 28, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 2 semana(s) el lunes, jueves, viernes. Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 28/01/2021 a las 8:00 comenzando el 01/01/2021");


            // ENGLISH UK
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 14/01/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 14/01/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 14/01/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 15/01/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 15/01/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 15/01/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/01/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/01/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/01/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 28/01/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 28, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 28/01/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 28, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 28/01/2021 at 08:00 starting on 01/01/2021");

            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/14/2021 at 4:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/14/2021 at 6:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/14/2021 at 8:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/15/2021 at 4:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/15/2021 at 6:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/15/2021 at 8:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/25/2021 at 4:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/25/2021 at 6:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/25/2021 at 8:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 25, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/28/2021 at 4:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 28, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/28/2021 at 6:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 28, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 28, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs every 2 week(s) on monday, thursday, friday. Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/28/2021 at 8:00 AM starting on 1/1/2021");

        }
        [Fact]
        public void TypeWeekly_NoHourlyAllWeekDays()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Weekly;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.WeekDays = (int)(WeekDaysEnum.Monday | WeekDaysEnum.Tuesday | WeekDaysEnum.Wednesday | WeekDaysEnum.Thursday | 
            WeekDaysEnum.Friday | WeekDaysEnum.Saturday | WeekDaysEnum.Sunday);
            sm.Periodicity = 1;

            // SPANISH
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 14/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 15/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 16, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 16/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 17, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 17/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 18/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 19/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 19, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 20/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 20, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 21, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 21/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 21, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 22, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 22/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 22, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 23, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 23/01/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 23, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 24, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre cada 1 semana(s) el lunes, martes, miercoles, jueves, viernes, sabado, domingo. La siguiente cita se producirá el 24/01/2021 a las 0:00 comenzando el 01/01/2021");

            // ENGLISH UK
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 14/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 15/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 16, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 16/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 17, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 17/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 18/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 19/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 19, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 20/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 20, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 21, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 21/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 21, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 22, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 22/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 22, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 23, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 23/01/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 1, 23, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 24, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 24/01/2021 at 00:00 starting on 01/01/2021");

            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2021, 1, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/14/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 14, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 15, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/15/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 15, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 16, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/16/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 16, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 17, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/17/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/18/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/19/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 19, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/20/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 20, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 21, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/21/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 21, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 22, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/22/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 22, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 23, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/23/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 1, 23, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 1, 24, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs every 1 week(s) on monday, tuesday, wednesday, thursday, friday, saturday, sunday. Schedule will be used on 1/24/2021 at 12:00 AM starting on 1/1/2021");

        }
        [Fact]
        public void TypeMonthly_NoHourly_FixedDay()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Monthly;
            sm.MonthlyFrecuency = MonthlyFrecuencyEnum.FixedDay;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;

            // COMPRUEBA QUE SE LANZA LA EXCEPCION DE DIA INCORRECTO
            sm.MonthlyFixedDay = 32;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            Exception ex = Assert.Throws<SchedulerException>(() => sm.NextOcurrence());
            Assert.Equal("El día debe del mes estar comprendido entre 1 y 31.", ex.Message);

            // SPANISH
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.MonthlyFixedDay = 31;
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). La siguiente cita se producirá el 31/01/2020 a las 0:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3,31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). La siguiente cita se producirá el 31/03/2020 a las 0:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). La siguiente cita se producirá el 31/05/2020 a las 0:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). La siguiente cita se producirá el 31/10/2020 a las 0:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). La siguiente cita se producirá el 31/12/2020 a las 0:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 12, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 2, 28, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). La siguiente cita se producirá el 28/02/2021 a las 0:00 comenzando el 01/01/2020");

            // ENGLISH UK
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.MonthlyFixedDay = 31;
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 31/01/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 31/03/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 31/05/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 31/10/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 31/12/2020 at 00:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 12, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 2, 28, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 28/02/2021 at 00:00 starting on 01/01/2020");

            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.MonthlyFixedDay = 31;
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 1/31/2020 at 12:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 3/31/2020 at 12:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 5/31/2020 at 12:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 10/31/2020 at 12:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 12/31/2020 at 12:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 12, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 2, 28, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Schedule will be used on 2/28/2021 at 12:00 AM starting on 1/1/2020");
        }
        [Fact]
        public void TypeMonthly_NoHourly_DayPosition()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Monthly;
            sm.MonthlyFrecuency = MonthlyFrecuencyEnum.DayPosition;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;

            // SPANISH
            // FIRST MONDAY EVERY 2 MONTHS
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12 ,6, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer lunes de cada 2 mes(es). La siguiente cita se producirá el 06/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 7, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer lunes de cada 2 mes(es). La siguiente cita se producirá el 07/02/2022 a las 0:00 comenzando el 01/01/2021");

            // SECOND MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo lunes de cada 2 mes(es). La siguiente cita se producirá el 13/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo lunes de cada 2 mes(es). La siguiente cita se producirá el 14/02/2022 a las 0:00 comenzando el 01/01/2021");

            // THIRD MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer lunes de cada 2 mes(es). La siguiente cita se producirá el 18/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer lunes de cada 2 mes(es). La siguiente cita se producirá el 20/12/2021 a las 0:00 comenzando el 01/01/2021");

            // FOURTH MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto lunes de cada 2 mes(es). La siguiente cita se producirá el 25/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto lunes de cada 2 mes(es). La siguiente cita se producirá el 27/12/2021 a las 0:00 comenzando el 01/01/2021");

            // LAST MONDAY EVERY 2 MONTHSş
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo lunes de cada 2 mes(es). La siguiente cita se producirá el 25/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo lunes de cada 2 mes(es). La siguiente cita se producirá el 27/12/2021 a las 0:00 comenzando el 01/01/2021");


            // FIRST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer sabado de cada 2 mes(es). La siguiente cita se producirá el 04/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer sabado de cada 2 mes(es). La siguiente cita se producirá el 05/02/2022 a las 0:00 comenzando el 01/01/2021");

            // SECOND SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo sabado de cada 2 mes(es). La siguiente cita se producirá el 11/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo sabado de cada 2 mes(es). La siguiente cita se producirá el 12/02/2022 a las 0:00 comenzando el 01/01/2021");

            // THIRD SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer sabado de cada 2 mes(es). La siguiente cita se producirá el 18/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer sabado de cada 2 mes(es). La siguiente cita se producirá el 19/02/2022 a las 0:00 comenzando el 01/01/2021");

            // FOURTH SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto sabado de cada 2 mes(es). La siguiente cita se producirá el 23/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto sabado de cada 2 mes(es). La siguiente cita se producirá el 25/12/2021 a las 0:00 comenzando el 01/01/2021");

            // LAST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo sabado de cada 2 mes(es). La siguiente cita se producirá el 30/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo sabado de cada 2 mes(es). La siguiente cita se producirá el 25/12/2021 a las 0:00 comenzando el 01/01/2021");

            // FIRST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 04/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 05/02/2022 a las 0:00 comenzando el 01/01/2021");

            // SECOND WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 05/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 6, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 06/02/2022 a las 0:00 comenzando el 01/01/2021");

            // THIRD WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 11/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 12/02/2022 a las 0:00 comenzando el 01/01/2021");

            // FOURTH WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 12/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 13/02/2022 a las 0:00 comenzando el 01/01/2021");

            // LAST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 31/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 26, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia del fin de semana de cada 2 mes(es). La siguiente cita se producirá el 26/12/2021 a las 0:00 comenzando el 01/01/2021");


            // TODOS LOS DIAS DEL MES
            // FIRST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia de cada 2 mes(es). La siguiente cita se producirá el 01/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia de cada 2 mes(es). La siguiente cita se producirá el 01/02/2022 a las 0:00 comenzando el 01/01/2021");

            // LAST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia de cada 2 mes(es). La siguiente cita se producirá el 31/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia de cada 2 mes(es). La siguiente cita se producirá el 31/12/2021 a las 0:00 comenzando el 01/01/2021");

            // DIAS DE LA SEMANA LABORAL
            // FIRST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia laboral de cada 2 mes(es). La siguiente cita se producirá el 01/12/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia laboral de cada 2 mes(es). La siguiente cita se producirá el 01/02/2022 a las 0:00 comenzando el 01/01/2021");

            // LAST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10,29, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia laboral de cada 2 mes(es). La siguiente cita se producirá el 29/10/2021 a las 0:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia laboral de cada 2 mes(es). La siguiente cita se producirá el 31/12/2021 a las 0:00 comenzando el 01/01/2021");

            // ENGLISH UK
            // FIRST MONDAY EVERY 2 MONTHS
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Schedule will be used on 06/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 7, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Schedule will be used on 07/02/2022 at 00:00 starting on 01/01/2021");

            // SECOND MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Schedule will be used on 13/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Schedule will be used on 14/02/2022 at 00:00 starting on 01/01/2021");

            // THIRD MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Schedule will be used on 18/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Schedule will be used on 20/12/2021 at 00:00 starting on 01/01/2021");

            // FOURTH MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Schedule will be used on 25/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Schedule will be used on 27/12/2021 at 00:00 starting on 01/01/2021");

            // LAST MONDAY EVERY 2 MONTHSş
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Schedule will be used on 25/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Schedule will be used on 27/12/2021 at 00:00 starting on 01/01/2021");


            // FIRST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Schedule will be used on 04/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Schedule will be used on 05/02/2022 at 00:00 starting on 01/01/2021");

            // SECOND SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Schedule will be used on 11/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Schedule will be used on 12/02/2022 at 00:00 starting on 01/01/2021");

            // THIRD SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Schedule will be used on 18/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Schedule will be used on 19/02/2022 at 00:00 starting on 01/01/2021");

            // FOURTH SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Schedule will be used on 23/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Schedule will be used on 25/12/2021 at 00:00 starting on 01/01/2021");

            // LAST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Schedule will be used on 30/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Schedule will be used on 25/12/2021 at 00:00 starting on 01/01/2021");

            // FIRST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Schedule will be used on 04/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Schedule will be used on 05/02/2022 at 00:00 starting on 01/01/2021");

            // SECOND WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Schedule will be used on 05/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 6, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Schedule will be used on 06/02/2022 at 00:00 starting on 01/01/2021");

            // THIRD WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Schedule will be used on 11/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Schedule will be used on 12/02/2022 at 00:00 starting on 01/01/2021");

            // FOURTH WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Schedule will be used on 12/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Schedule will be used on 13/02/2022 at 00:00 starting on 01/01/2021");

            // LAST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Schedule will be used on 31/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 26, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Schedule will be used on 26/12/2021 at 00:00 starting on 01/01/2021");


            // TODOS LOS DIAS DEL MES
            // FIRST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Schedule will be used on 01/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Schedule will be used on 01/02/2022 at 00:00 starting on 01/01/2021");

            // LAST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Schedule will be used on 31/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Schedule will be used on 31/12/2021 at 00:00 starting on 01/01/2021");

            // DIAS DE LA SEMANA LABORAL
            // FIRST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Schedule will be used on 01/12/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Schedule will be used on 01/02/2022 at 00:00 starting on 01/01/2021");

            // LAST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Schedule will be used on 29/10/2021 at 00:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Schedule will be used on 31/12/2021 at 00:00 starting on 01/01/2021");

            // ENGLISH US
            // FIRST MONDAY EVERY 2 MONTHS
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Schedule will be used on 12/6/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 7, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Schedule will be used on 2/7/2022 at 12:00 AM starting on 1/1/2021");

            // SECOND MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Schedule will be used on 12/13/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 14, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Schedule will be used on 2/14/2022 at 12:00 AM starting on 1/1/2021");

            // THIRD MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Schedule will be used on 10/18/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 20, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Schedule will be used on 12/20/2021 at 12:00 AM starting on 1/1/2021");

            // FOURTH MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Schedule will be used on 10/25/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Schedule will be used on 12/27/2021 at 12:00 AM starting on 1/1/2021");

            // LAST MONDAY EVERY 2 MONTHSş
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Schedule will be used on 10/25/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Schedule will be used on 12/27/2021 at 12:00 AM starting on 1/1/2021");


            // FIRST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Schedule will be used on 12/4/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Schedule will be used on 2/5/2022 at 12:00 AM starting on 1/1/2021");

            // SECOND SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Schedule will be used on 12/11/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Schedule will be used on 2/12/2022 at 12:00 AM starting on 1/1/2021");

            // THIRD SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Schedule will be used on 12/18/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 19, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Schedule will be used on 2/19/2022 at 12:00 AM starting on 1/1/2021");

            // FOURTH SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Schedule will be used on 10/23/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Schedule will be used on 12/25/2021 at 12:00 AM starting on 1/1/2021");

            // LAST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Schedule will be used on 10/30/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Schedule will be used on 12/25/2021 at 12:00 AM starting on 1/1/2021");

            // FIRST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Schedule will be used on 12/4/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Schedule will be used on 2/5/2022 at 12:00 AM starting on 1/1/2021");

            // SECOND WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Schedule will be used on 12/5/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 6, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Schedule will be used on 2/6/2022 at 12:00 AM starting on 1/1/2021");

            // THIRD WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Schedule will be used on 12/11/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Schedule will be used on 2/12/2022 at 12:00 AM starting on 1/1/2021");

            // FOURTH WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Schedule will be used on 12/12/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 13, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Schedule will be used on 2/13/2022 at 12:00 AM starting on 1/1/2021");

            // LAST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Schedule will be used on 10/31/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 26, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Schedule will be used on 12/26/2021 at 12:00 AM starting on 1/1/2021");
            

            // TODOS LOS DIAS DEL MES
            // FIRST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Schedule will be used on 12/1/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Schedule will be used on 2/1/2022 at 12:00 AM starting on 1/1/2021");

            // LAST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Schedule will be used on 10/31/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Schedule will be used on 12/31/2021 at 12:00 AM starting on 1/1/2021");

            // DIAS DE LA SEMANA LABORAL
            // FIRST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Schedule will be used on 12/1/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Schedule will be used on 2/1/2022 at 12:00 AM starting on 1/1/2021");

            // LAST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Schedule will be used on 10/29/2021 at 12:00 AM starting on 1/1/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 0, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Schedule will be used on 12/31/2021 at 12:00 AM starting on 1/1/2021");

        }
        [Fact]
        public void TypeMonthly_Hourly_FixedDay()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Monthly;
            sm.MonthlyFrecuency = MonthlyFrecuencyEnum.FixedDay;
            sm.StartDate = new DateTime(2020, 1, 1, 0, 0, 0);
            sm.MonthlyFixedDay = 31;
            sm.Periodicity = 2;
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyStartAt = new DateTime(1001, 1, 1, 4, 0, 0);
            sm.HourlyEndAt = new DateTime(1001, 1, 1, 8, 0, 0);
            sm.HourlyOccursEvery = 2;

            // SPANISH
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/01/2020 a las 4:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1,31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/01/2020 a las 6:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/01/2020 a las 8:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/03/2020 a las 4:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/03/2020 a las 6:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/03/2020 a las 8:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/05/2020 a las 4:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/05/2020 a las 6:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/05/2020 a las 8:00 comenzando el 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 7, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el dia 31 de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/07/2020 a las 4:00 comenzando el 01/01/2020");

            // ENGLISH UK
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/01/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/01/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/01/2020 at 08:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/03/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/03/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/03/2020 at 08:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/05/2020 at 04:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/05/2020 at 06:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/05/2020 at 08:00 starting on 01/01/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 7, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/07/2020 at 04:00 starting on 01/01/2020");

            // ENGLISH US
            sm.Language = SupportedLanguagesEnum.en_US;
            sm.CurrentDate = new DateTime(2020, 1, 1, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/31/2020 at 4:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/31/2020 at 6:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 1, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 1/31/2020 at 8:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 1, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 3/31/2020 at 4:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 3/31/2020 at 6:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 3, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 3/31/2020 at 8:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 3, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 5/31/2020 at 4:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 5/31/2020 at 6:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 5, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 5/31/2020 at 8:00 AM starting on 1/1/2020");

            sm.CurrentDate = new DateTime(2020, 5, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2020, 7, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs on day 31 of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 7/31/2020 at 4:00 AM starting on 1/1/2020");

            

        }
        [Fact]
        public void TypeMonthly_Hourly_DayPosition()
        {
            SchedulerManager sm = new SchedulerManager();
            sm.Type = RecurrenceTypesEnum.Monthly;
            sm.MonthlyFrecuency = MonthlyFrecuencyEnum.DayPosition;
            sm.StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
            sm.Periodicity = 2;
            sm.HourlyStartAt = new DateTime(1001, 1, 1, 4, 0, 0);
            sm.HourlyEndAt  = new DateTime(1001, 1, 1, 8, 0, 0);
            sm.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
            sm.HourlyOccursEvery = 2;



            // SPANISH
            // FIRST MONDAY EVERY 2 MONTHS
            sm.Language = SupportedLanguagesEnum.es_ES;
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            var data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 06/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 06/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 06/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 7, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 07/02/2022 a las 4:00 comenzando el 01/01/2021");


            // SECOND MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 14, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 14/02/2022 a las 4:00 comenzando el 01/01/2021");


            // THIRD MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 18/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 18/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 18/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 20, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 20/12/2021 a las 4:00 comenzando el 01/01/2021");



            // FOURTH MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 27/12/2021 a las 4:00 comenzando el 01/01/2021");


            // LAST MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 17, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 17, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 17, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo lunes de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 27/12/2021 a las 4:00 comenzando el 01/01/2021");



            // FIRST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 04/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 04/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 04/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 05/02/2022 a las 4:00 comenzando el 01/01/2021");


            // SECOND SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 11/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 11/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 11/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 12/02/2022 a las 4:00 comenzando el 01/01/2021");


            // THIRD SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 18/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 18/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 18/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 19, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 19/02/2022 a las 4:00 comenzando el 01/01/2021");


            // FOURTH SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 23/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 23/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 23/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/12/2021 a las 4:00 comenzando el 01/01/2021");


            // LAST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 30/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 30/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 30/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo sabado de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 25/12/2021 a las 4:00 comenzando el 01/01/2021");


            // FIRST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 04/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 04/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 04/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 05/02/2022 a las 4:00 comenzando el 01/01/2021");


            // SECOND WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 05/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 05/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 05/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 6, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el segundo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 06/02/2022 a las 4:00 comenzando el 01/01/2021");


            // THIRD WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 11/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 11/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 11/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el tercer dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 12/02/2022 a las 4:00 comenzando el 01/01/2021");


            // FOURTH WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 12/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 12/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 12/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el cuarto dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 13/02/2022 a las 4:00 comenzando el 01/01/2021");


            // LAST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 26, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia del fin de semana de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 26/12/2021 a las 4:00 comenzando el 01/01/2021");


            // TODOS LOS DIAS DEL MES
            // FIRST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/02/2022 a las 4:00 comenzando el 01/01/2021");


            // LAST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/12/2021 a las 4:00 comenzando el 01/01/2021");


            // DIAS DE LA SEMANA LABORAL
            // FIRST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/12/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/12/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/12/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el primer dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 01/02/2022 a las 4:00 comenzando el 01/01/2021");


            // LAST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 29/10/2021 a las 4:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 6, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 29/10/2021 a las 6:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 8, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 29/10/2021 a las 8:00 comenzando el 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Ocurre el ultimo dia laboral de cada 2 mes(es). Cada 2 hora(s) entre 04:00 y 08:00. La siguiente cita se producirá el 31/12/2021 a las 4:00 comenzando el 01/01/2021");

            // ENGLISH UK
            // FIRST MONDAY EVERY 2 MONTHS
            sm.Language = SupportedLanguagesEnum.en_GB;
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 06/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 06/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 6, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 06/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 6, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 7, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 07/02/2022 at 04:00 starting on 01/01/2021");


            // SECOND MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 13, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 13, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 14, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the second monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 14/02/2022 at 04:00 starting on 01/01/2021");


            // THIRD MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 18/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 18/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 18, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 18/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 18, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 20, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the third monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 20/12/2021 at 04:00 starting on 01/01/2021");



            // FOURTH MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 25, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 27/12/2021 at 04:00 starting on 01/01/2021");


            // LAST MONDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Monday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 17, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 17, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 25, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 17, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 27, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last monday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 27/12/2021 at 04:00 starting on 01/01/2021");



            // FIRST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 04/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 04/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 04/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 05/02/2022 at 04:00 starting on 01/01/2021");


            // SECOND SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 11/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 11/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 11/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the second saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 12/02/2022 at 04:00 starting on 01/01/2021");


            // THIRD SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 18/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 18/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 18, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 18/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 18, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 19, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the third saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 19/02/2022 at 04:00 starting on 01/01/2021");


            // FOURTH SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 23/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 23/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 23, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 23/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 23, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/12/2021 at 04:00 starting on 01/01/2021");


            // LAST SATURDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Saturday;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 30/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 30/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 30, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 30/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 30, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 25, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last saturday of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 25/12/2021 at 04:00 starting on 01/01/2021");


            // FIRST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 04/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 04/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 4, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 04/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 4, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 5, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 05/02/2022 at 04:00 starting on 01/01/2021");


            // SECOND WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Second;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 05/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 05/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 5, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 05/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 5, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 6, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the second weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 06/02/2022 at 04:00 starting on 01/01/2021");


            // THIRD WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Third;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 11/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 11/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 11, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 11/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 11, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 12, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the third weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 12/02/2022 at 04:00 starting on 01/01/2021");


            // FOURTH WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Fourth;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 12/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 12/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 12, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 12/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 12, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 13, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the fourth weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 13/02/2022 at 04:00 starting on 01/01/2021");


            // LAST WEEKENDDAYS EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekendDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 26, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekenddays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 26/12/2021 at 04:00 starting on 01/01/2021");


            // TODOS LOS DIAS DEL MES
            // FIRST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/02/2022 at 04:00 starting on 01/01/2021");


            // LAST DAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.Day;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 31, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 31, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last day of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/12/2021 at 04:00 starting on 01/01/2021");


            // DIAS DE LA SEMANA LABORAL
            // FIRST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.First;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/12/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/12/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 1, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/12/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 12, 1, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2022, 2, 1, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the first weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 01/02/2022 at 04:00 starting on 01/01/2021");


            // LAST WEEKDAY EVERY 2 MONTHS
            sm.MonthlyDayPosition = DayPositionEnum.Last;
            sm.WeekDays = (int)WeekDaysEnum.WeekDays;
            sm.CurrentDate = new DateTime(2021, 10, 17, 0, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 29/10/2021 at 04:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 4, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 6, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 29/10/2021 at 06:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 6, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 10, 29, 8, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 29/10/2021 at 08:00 starting on 01/01/2021");

            sm.CurrentDate = new DateTime(2021, 10, 29, 8, 0, 0);
            data = sm.NextOcurrence();
            Assert.Equal(data.OcurrenceDate, new DateTime(2021, 12, 31, 4, 0, 0));
            Assert.Equal(data.Description, "Occurs the last weekdays of every 2 month(s). Every 2 hour(s) between 04:00 and 08:00. Schedule will be used on 31/12/2021 at 04:00 starting on 01/01/2021");

        }

    }

}
