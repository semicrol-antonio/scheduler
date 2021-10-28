using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Scheduler;


namespace Scheduler

{
    public static class SchedulerManager
    {
        private static SchedulerConfiguration conf;
        public static List<OutData> CalculateSerie(this SchedulerConfiguration sc)

        {
            int Ocurrences = 0;
            var salida = new List<OutData>();
            OutData ocurrencia;

            conf = sc;
            while (true)
            {
                ocurrencia = NextOcurrence();
                if (sc.Range == RangeEnum.OcurrenceCount)
                {
                    Ocurrences++;
                    if (Ocurrences > sc.OcurrenceCount)
                        break;
                }
                else
                {
                    if (ocurrencia.OcurrenceDate >= sc.EndDate)
                        break;
                }
                salida.Add(ocurrencia);
                sc.CurrentDate = ocurrencia.OcurrenceDate;
            }
            return salida;
        }

        private static OutData NextOcurrence()
        {
            OutData salida = new OutData();

            switch (conf.Type)
            {
                case RecurrenceTypesEnum.Once:
                    InputValidationsGeneral();
                    salida.OcurrenceDate = OneOcurrence();
                    break;
                case RecurrenceTypesEnum.Daily:
                    InputValidationsDaily();
                    salida.OcurrenceDate = DailyRecurrence(conf.CurrentDate, conf.Periodicity);
                    break;
                case RecurrenceTypesEnum.Weekly:
                    InputValidationsWeekly();
                    salida.OcurrenceDate = WeeklyRecurrence();
                    break;
                case RecurrenceTypesEnum.Monthly:
                    InputValidationsMonthly();
                    salida.OcurrenceDate = MonthlyRecurrence();
                    break;
            }

            salida.NextExecutionTime = conf.LanguageManager.GetLanguageDateTimeFormatted(salida.OcurrenceDate);
            salida.Description = DescriptionMount(salida);

            return salida;
        }
        private static DateTime OneOcurrence()
        {
            DateTime salida;

            salida = conf.CurrentDate;

            return salida;
        }
        private static DateTime HourlyRecurrence(DateTime CurrentDate, bool First)
        {
            DateTime WorkDateTime;

            WorkDateTime = CurrentDate;
            switch (conf.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    if (new DateTime(1001, 1, 1, WorkDateTime.Hour, WorkDateTime.Minute, 0) < new DateTime(1001, 1, 1, conf.HourlyOccursAt.Hour, conf.HourlyOccursAt.Minute, 0))
                        WorkDateTime = WorkDateTime.SetTime(conf.HourlyOccursAt.Hour, conf.HourlyOccursAt.Minute, conf.HourlyOccursAt.Second);
                    else
                        WorkDateTime = DateTime.MinValue;
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    if (First)
                    {
                        if (WorkDateTime.Hour == 0 && conf.HourlyStartAt.Hour == 0)
                            return WorkDateTime;
                    }
                    int NextHour = NextHourInterval(WorkDateTime.Hour);
                    if (NextHour != -1)
                        WorkDateTime = WorkDateTime.SetTime(NextHour);
                    else
                        WorkDateTime = DateTime.MinValue;
                    break;
            }
            return WorkDateTime;
        }

        private static DateTime DailyRecurrence(DateTime CurrentDate, int DailyPeriodicity)
        {
            DateTime Salida;
            DateTime WorkDateTime;

            WorkDateTime = CurrentDate;
            switch (conf.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    Salida = HourlyRecurrence(WorkDateTime, false);
                    if (Salida != DateTime.MinValue)
                        WorkDateTime = Salida;
                    else
                    {
                        WorkDateTime = CurrentDate.AddDays(DailyPeriodicity);
                        WorkDateTime = WorkDateTime.SetTime(conf.HourlyOccursAt.Hour, conf.HourlyOccursAt.Minute, conf.HourlyOccursAt.Second);
                    }
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    Salida = HourlyRecurrence(WorkDateTime, false);
                    if (Salida != DateTime.MinValue)
                        WorkDateTime = Salida;
                    else
                    {
                        WorkDateTime = WorkDateTime.AddDays(conf.Periodicity);
                        WorkDateTime = WorkDateTime.SetTime(conf.HourlyStartAt.Hour, 0);
                    }
                    break;
                default:
                    WorkDateTime = WorkDateTime.AddDays(conf.Periodicity);
                    break;
            }
            return WorkDateTime;
        }
        private static DateTime WeeklyRecurrence()
        {
            DateTime WorkDateTime;
            bool Processed = false;
            int CurrentWeekNumber;


            WorkDateTime = conf.CurrentDate;
            CurrentWeekNumber = GetWeekNumber(WorkDateTime);
            while (Processed == false)
            {
                if (DayOfWeekIncluded(WorkDateTime))
                {
                    if (conf.HourlyFrecuency != HourlyFrecuencysEnum.None)
                    {
                        var tmp_datetime = HourlyRecurrence(WorkDateTime, WorkDateTime != conf.CurrentDate);
                        if (tmp_datetime != DateTime.MinValue)
                        {
                            WorkDateTime = tmp_datetime;
                            break;
                        }

                    }
                    else
                        if (WorkDateTime != conf.CurrentDate)
                        {
                            WorkDateTime = WorkDateTime.SetTime(conf.CurrentDate.Hour, conf.CurrentDate.Minute, conf.CurrentDate.Second);
                            break;
                        }
                        
                }
                WorkDateTime = WorkDateTime.AddDays(1);
                WorkDateTime = WorkDateTime.SetTime(0, 0, 0);

                if (CurrentWeekNumber != GetWeekNumber(WorkDateTime))
                {
                    WorkDateTime = WorkDateTime.AddDays(7 * (-1));   // Ajusta al lunes de la semana actual
                    WorkDateTime = WorkDateTime.AddDays(7 * conf.Periodicity);  // Avanza las semanas indicadas
                    CurrentWeekNumber = GetWeekNumber(WorkDateTime);
                }
            }

            return WorkDateTime;
        }
        private static DateTime MonthlyRecurrence()
        {
            DateTime WorkDateTime;
            DateTime datetime_tmp;

            WorkDateTime = conf.CurrentDate;

            datetime_tmp = DateTime.MinValue;
            while (true)
            {
                if (conf.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay)
                    datetime_tmp = GetDateByDayFixed(conf.MonthlyFixedDay, WorkDateTime);
                else
                    datetime_tmp = GetDateByDayPosition(conf.MonthlyDayPosition, conf.WeekDays, WorkDateTime);
                if (WorkDateTime <= datetime_tmp)
                {
                    WorkDateTime = datetime_tmp;
                    if (WorkDateTime != conf.CurrentDate || conf.HourlyFrecuency != HourlyFrecuencysEnum.None)
                    {
                        if (conf.HourlyFrecuency != HourlyFrecuencysEnum.None)
                        {
                            var tmp_datetime = HourlyRecurrence(WorkDateTime, WorkDateTime != conf.CurrentDate);
                            if (tmp_datetime != DateTime.MinValue)
                            {
                                WorkDateTime = tmp_datetime;
                                break;
                            }

                        }
                        else
                            break;

                    }
                }
                WorkDateTime = new DateTime(conf.CurrentDate.Year, conf.CurrentDate.Month, 1, 0, 0, 0);
                WorkDateTime = WorkDateTime.AddMonths(conf.Periodicity);
            }

            return WorkDateTime;
        }
        private static DateTime GetDateByDayFixed(int FixedDay, DateTime BeginDate)
        {
            DateTime WorkDateTime;
            int day_tmp;
            if (FixedDay != BeginDate.Day)
            {
                if (FixedDay > BeginDate.LastDay(BeginDate).Day)
                    day_tmp = BeginDate.LastDay(BeginDate).Day;
                else
                    day_tmp = FixedDay;

                WorkDateTime = new DateTime(BeginDate.Year, BeginDate.Month, day_tmp, BeginDate.Hour, BeginDate.Minute, BeginDate.Second);
            }
            else
                WorkDateTime = BeginDate;

            return WorkDateTime;
        }
        private static DateTime GetDateByDayPosition(DayPositionEnum search_position, int weekDays, DateTime WorkDateTime)
        {
            int Position = 99;
            DateTime LastDayFound = DateTime.MinValue;
            int ActualMonth = WorkDateTime.Month;
            int Contador = 0;

            if (search_position != DayPositionEnum.Last)
                Position = (int)search_position + 1;

            WorkDateTime = new DateTime(WorkDateTime.Year, WorkDateTime.Month, 1, WorkDateTime.Hour, WorkDateTime.Minute, WorkDateTime.Second);

            // Procesa todos los días del mes de la fecha recibida
            while (ActualMonth == WorkDateTime.Month)
            {
                // Si el dia actual es lunes comprueba si se esta buscando el lunes, Dia de la semana de trabajo o cualquier dia. Lo mismo para el resto de los dias de la semana (Salvo sabados y domingos)
                if ((WorkDateTime.DayOfWeek == DayOfWeek.Monday && (weekDays == (int)WeekDaysEnum.Monday || weekDays == (int)WeekDaysEnum.WeekDays || weekDays == (int)WeekDaysEnum.Day)) ||
                (WorkDateTime.DayOfWeek == DayOfWeek.Tuesday && (weekDays == (int)WeekDaysEnum.Tuesday || weekDays == (int)WeekDaysEnum.WeekDays || weekDays == (int)WeekDaysEnum.Day)) ||
                (WorkDateTime.DayOfWeek == DayOfWeek.Wednesday && (weekDays == (int)WeekDaysEnum.Wednesday || weekDays == (int)WeekDaysEnum.WeekDays || weekDays == (int)WeekDaysEnum.Day)) ||
                (WorkDateTime.DayOfWeek == DayOfWeek.Thursday && (weekDays == (int)WeekDaysEnum.Thursday || weekDays == (int)WeekDaysEnum.WeekDays || weekDays == (int)WeekDaysEnum.Day)) ||
                (WorkDateTime.DayOfWeek == DayOfWeek.Friday && (weekDays == (int)WeekDaysEnum.Friday || weekDays == (int)WeekDaysEnum.WeekDays || weekDays == (int)WeekDaysEnum.Day)) ||
                (WorkDateTime.DayOfWeek == DayOfWeek.Saturday && (weekDays == (int)WeekDaysEnum.Saturday || weekDays == (int)WeekDaysEnum.WeekendDays || weekDays == (int)WeekDaysEnum.Day)) ||
                (WorkDateTime.DayOfWeek == DayOfWeek.Sunday && (weekDays == (int)WeekDaysEnum.Sunday || weekDays == (int)WeekDaysEnum.WeekendDays || weekDays == (int)WeekDaysEnum.Day)))
                {
                    Contador++;
                    LastDayFound = WorkDateTime;
                }
                if (Contador == (int)Position)
                    break;

                WorkDateTime = WorkDateTime.AddDays(1);

            }
            if (Position == 99)
                WorkDateTime = LastDayFound;

            return WorkDateTime;
        }
        private static bool DayOfWeekIncluded(DateTime WorkDateTime)
        {
            bool Result = false;

            if ((WorkDateTime.DayOfWeek == DayOfWeek.Monday && (conf.WeekDays & (int)WeekDaysEnum.Monday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Tuesday && (conf.WeekDays & (int)WeekDaysEnum.Tuesday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Wednesday && (conf.WeekDays & (int)WeekDaysEnum.Wednesday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Thursday && (conf.WeekDays & (int)WeekDaysEnum.Thursday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Friday && (conf.WeekDays & (int)WeekDaysEnum.Friday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Saturday && (conf.WeekDays & (int)WeekDaysEnum.Saturday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Sunday && (conf.WeekDays & (int)WeekDaysEnum.Sunday) != 0))
            {
                Result = true;
            }

            return Result;
        }
        private static int GetWeekNumber(DateTime ActualDate)
        {
            int weekNum = conf.LanguageManager.GetWeekOfYear(ActualDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        private static string DescriptionMount(OutData salida)
        {
            string Description = "";

            switch (conf.Type)
            {
                case RecurrenceTypesEnum.Once:
                    Description = conf.LanguageManager.TraslateTag("occursone");
                    break;
                case RecurrenceTypesEnum.Daily:
                    Description = conf.LanguageManager.TraslateTag("occursevery") + " " + conf.Periodicity + " " + conf.LanguageManager.TraslateTag("days");
                    break;
                case RecurrenceTypesEnum.Weekly:
                    Description = conf.LanguageManager.TraslateTag("occursevery") + " " + conf.Periodicity + " " + conf.LanguageManager.TraslateTag("weeks") + " " +
                                  conf.LanguageManager.TraslateTag("on") + " " + StringDays();
                    break;
                case RecurrenceTypesEnum.Monthly:
                    if (conf.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay)
                    {
                        Description = conf.LanguageManager.TraslateTag("occursonday") + " " + conf.MonthlyFixedDay;
                    }
                    else
                    {
                        Description = conf.LanguageManager.TraslateTag("occursthe") + " " +
                                      conf.LanguageManager.TraslateTag(Enum.GetName(typeof(DayPositionEnum), conf.MonthlyDayPosition).ToLower()) + " " +
                                      conf.LanguageManager.TraslateTag(Enum.GetName(typeof(WeekDaysEnum), (WeekDaysEnum)conf.WeekDays).ToLower());
                    }
                    Description += " "+conf.LanguageManager.TraslateTag("ofevery")+" "+ conf.Periodicity + " "+ conf.LanguageManager.TraslateTag("months");
                    break;

            }
            switch (conf.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    Description += " "+conf.LanguageManager.TraslateTag("at")+" " + conf.LanguageManager .GetLanguageTimeFormatted (conf.HourlyOccursAt);
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    Description += ". "+ conf.LanguageManager.TraslateTag("every")+" " + conf.HourlyOccursEvery + " "+
                                         conf.LanguageManager.TraslateTag("hours")+" " + conf.LanguageManager.TraslateTag("between")+" " +
                                         conf.LanguageManager.GetLanguageTimeFormatted(conf.HourlyStartAt) + " "+ conf.LanguageManager.TraslateTag("and")+" " + conf.LanguageManager.GetLanguageTimeFormatted(conf.HourlyEndAt);
                    break;
                default:
                    break;
            }
            Description += ". "+ conf.LanguageManager.TraslateTag("schedulewillbeusedon")+" " + 
                                 conf.LanguageManager.GetLanguageDateFormatted(salida.OcurrenceDate) + " "+
                                 conf.LanguageManager.TraslateTag("at")+" " + 
                                 conf.LanguageManager.GetLanguageTimeFormatted(salida.OcurrenceDate) + " "+
                                 conf.LanguageManager.TraslateTag("startingon")+" " + conf.LanguageManager.GetLanguageDateFormatted(conf.StartDate);

            return Description;
        }
        private static string StringDays()
        {
            string daystext = "";

            if ((conf.WeekDays & (int)WeekDaysEnum.Monday) != 0)
            {
                daystext = conf.LanguageManager .TraslateTag ("monday");
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Tuesday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += conf.LanguageManager.TraslateTag("tuesday");
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Wednesday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += conf.LanguageManager.TraslateTag("wednesday");
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Thursday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += conf.LanguageManager.TraslateTag("thursday");
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Friday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += conf.LanguageManager.TraslateTag("friday");
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Saturday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += conf.LanguageManager.TraslateTag("saturday");
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Sunday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += conf.LanguageManager.TraslateTag("sunday");
            }


            return daystext;
        }

        public static int NextHourInterval(int Hour)
        {
            int x;
            int NextHour = -1;

            x = conf.HourlyStartAt.Hour;
            while (x <= conf.HourlyEndAt.Hour)
            {
                if (Hour < x)
                {
                    NextHour = x;
                    break;
                }
                x += conf.HourlyOccursEvery;

            }
            return NextHour;
        }
        private static void InputValidationsGeneral()
        {
            // si El rango es por ocurrencecount y se envia a 0 se establece 1 ejecución por defecto
            if (conf.Range == RangeEnum.OcurrenceCount && conf.OcurrenceCount == 0)
                throw new SchedulerException("Debe indicarse el número de ocurrencias a obtener.");

            // Si se indica rango por fecha de fin y no se indica la fecha de fin
            if (conf.Range == RangeEnum.EndByDate && conf.EndDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicarse la fecha de finalización cuando el rango de ejecución está delimitado por la fecha hasta.");

            // Comprueba que se pase la fecha actual para el calculo de la siguiente ocurrencia
            if (conf.CurrentDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

            if (conf.StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            // Se comprueba el currentdate no sea inferior a la fecha de inicio
            if (conf.StartDate != DateTime.MinValue && conf.CurrentDate < conf.StartDate)
                throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");
        }
        private static void InputValidationsHourly()
        {
            if (conf.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery)
            {
                // Si se indica periodicidad horaria se comprueba que se han indicado la hora de limite desde-hasta
                if (conf.HourlyStartAt == DateTime.MinValue && conf.HourlyEndAt == DateTime.MinValue)
                    throw new SchedulerException("Debe indicarse las horas limite cuando el modo horario es periodico.");
                // Se comprueba que la hora desde no es superior a la hora hasta en modo hourly
                if (conf.HourlyStartAt >= conf.HourlyEndAt)
                    throw new SchedulerException("La hora 'desde' debe ser inferior a la hora 'hasta'.");
            }

            // Si estamos en modo hourly periodico se comprueba que se envia la periodicidad de horas
            if (conf.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery && conf.HourlyOccursEvery <= 0)
                throw new SchedulerException("Debe indicarse un valor para la periodicidad horaria.");
        }
        private static void InputValidationsDaily()
        {
            InputValidationsGeneral();
            InputValidationsHourly();
            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if (conf.Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");
        }
        private static void InputValidationsWeekly()
        {
            InputValidationsGeneral();
            InputValidationsHourly();
            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if (conf.Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");

            // Comprueba que se han indicado algun dia de la semana en la recurrencia semanal
            if (conf.WeekDays == 0)
                throw new SchedulerException("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.");

        }
        private static void InputValidationsMonthly()
        {
            InputValidationsGeneral();
            InputValidationsHourly();
            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if (conf.Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");

            // Comprobaciones recurrencia Mensual
            if (conf.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay && (conf.MonthlyFixedDay <= 0 || conf.MonthlyFixedDay > 31))
                throw new SchedulerException("El día debe del mes estar comprendido entre 1 y 31.");

        }
    }
}

