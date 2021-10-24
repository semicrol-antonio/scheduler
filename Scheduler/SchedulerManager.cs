using System;
using System.Collections;
using System.Globalization;
using Scheduler;


namespace Scheduler

{
    public class SchedulerException : Exception
    {
        public SchedulerException(String mensaje) : base(mensaje) { }
    }
    internal class SchedulerManager
    {
        private SchedulerConfiguration conf;
        public SchedulerManager(SchedulerConfiguration Configuration) 
        {
            conf = Configuration;
        }

        public OutData NextOcurrence()
        {
            InputValidations();


            OutData salida = new OutData();

            switch (conf.Type)
            {
                case RecurrenceTypesEnum.Once:
                    salida.OcurrenceDate = OneOcurrence();
                    break;
                case RecurrenceTypesEnum.Daily:
                    salida.OcurrenceDate = DailyRecurrence(conf.CurrentDate, conf.Periodicity);
                    break;
                case RecurrenceTypesEnum.Weekly:
                    salida.OcurrenceDate = WeeklyRecurrence();
                    break;
                case RecurrenceTypesEnum.Monthly:
                    salida.OcurrenceDate = MonthlyRecurrence();
                    break;

            }


            if (conf.EndDate != DateTime.MinValue && salida.OcurrenceDate > conf.EndDate)
                throw new SchedulerException("La ocurrencia calculada supera la fecha limite establecida.");

            salida.NextExecutionTime = conf.LanguageManager.GetLanguageDateTimeFormatted(salida.OcurrenceDate);
//            salida.NextExecutionTime = salida.OcurrenceDate.ToString("dd/MM/yyyy HH:mm");
            salida.Description = DescriptionMount(salida);

            return salida;
        }
        private DateTime OneOcurrence()
        {
            DateTime salida;

            salida = conf.CurrentDate;

            return salida;
        }
        private DateTime HourlyRecurrence(DateTime CurrentDate, bool First)
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

        private DateTime DailyRecurrence(DateTime CurrentDate, int DailyPeriodicity)
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
        private DateTime WeeklyRecurrence()
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
        private DateTime MonthlyRecurrence()
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
        private DateTime GetDateByDayFixed(int FixedDay, DateTime BeginDate)
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
        private DateTime GetDateByDayPosition(DayPositionEnum search_position, int weekDays, DateTime WorkDateTime)
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
        private bool DayOfWeekIncluded(DateTime WorkDateTime)
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
        private void InputValidations()
        {
            // Comprueba que se pase la fecha actual para el calculo de la siguiente ocurrencia
            if (conf.CurrentDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

            if (conf.StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if ((conf.Type == RecurrenceTypesEnum.Daily || conf.Type == RecurrenceTypesEnum.Weekly || conf.Type == RecurrenceTypesEnum.Monthly) && conf.Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");


            // Se comprueba el currentdate no sea inferior a la fecha de inicio
            if (conf.StartDate != DateTime.MinValue && conf.CurrentDate < conf.StartDate)
                throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");

            // Comprueba que se han indicado algun dia de la semana en la recurrencia semanal
            if (conf.Type == RecurrenceTypesEnum.Weekly && conf.WeekDays == 0)
                throw new SchedulerException("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.");

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

            // Comprobaciones recurrencia Mensual
            if (conf.Type == RecurrenceTypesEnum.Monthly)
            {
                if (conf.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay && (conf.MonthlyFixedDay <= 0 || conf.MonthlyFixedDay > 31))
                    throw new SchedulerException("El día debe del mes estar comprendido entre 1 y 31.");
            }
        }
        private int GetWeekNumber(DateTime ActualDate)
        {
            int weekNum = conf.LanguageManager.GetWeekOfYear(ActualDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        private string DescriptionMount(OutData salida)
        {
            string Description = "";

            switch (conf.Type)
            {
                case RecurrenceTypesEnum.Once:
                    Description = "Occurs One";
                    break;
                case RecurrenceTypesEnum.Daily:
                    Description = "Occurs every " + conf.Periodicity + " day(s)";
                    break;
                case RecurrenceTypesEnum.Weekly:
                    Description = "Occurs every " + conf.Periodicity + " week(s) on " + StringDays();
                    break;
                case RecurrenceTypesEnum.Monthly:
                    if (conf.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay)
                    {
                        Description = "Occurs on day " + conf.MonthlyFixedDay;
                    }
                    else
                    {
                        Description = "Occurs the " + Enum.GetName(typeof(DayPositionEnum), conf.MonthlyDayPosition).ToLower() + " " + Enum.GetName(typeof(WeekDaysEnum), (WeekDaysEnum)conf.WeekDays).ToLower();
                    }
                    Description += " of every " + conf.Periodicity + " month(s)";
                    break;

            }
            switch (conf.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    Description += " at " + conf.HourlyOccursAt.ToString("HH:mm");
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    Description += ". Every " + conf.HourlyOccursEvery + " hour(s) between " + conf.HourlyStartAt.ToString("HH:mm") + " and " + conf.HourlyEndAt.ToString("HH:mm");
                    break;
                default:
                    break;
            }
            Description += ". Schedule will be used on " + conf.LanguageManager.GetLanguageDateFormatted  (salida.OcurrenceDate) +
                            " at " + conf.LanguageManager.GetLanguageTimeFormatted(salida.OcurrenceDate) + " starting on " + conf.LanguageManager.GetLanguageDateFormatted (conf.StartDate);

            return conf.LanguageManager.Traslate(Description);
        }
        private string StringDays()
        {
            string daystext = "";

            if ((conf.WeekDays & (int)WeekDaysEnum.Monday) != 0)
            {
                daystext = "monday";
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Tuesday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "tuesday";
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Wednesday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "wednesday";
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Thursday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "thursday";
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Friday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "friday";
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Saturday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "saturday";
            }
            if ((conf.WeekDays & (int)WeekDaysEnum.Sunday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "sunday";
            }


            return daystext;
        }

        public int NextHourInterval(int Hour)
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
    }
}

