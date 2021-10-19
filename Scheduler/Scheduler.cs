using System;
using System.Globalization;

namespace Scheduler

{
    public enum RecurrenceTypesEnum
    {
        Once,
        Daily,
        Weekly,
        Monthly
    }
    public enum TimeIntervalEnum
    {
        Hours,
        Minutes,
        Seconds
    }
    public enum WeekDaysEnum
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,
        WeekDays = Monday | Tuesday | Wednesday | Thursday | Friday,
        WeekendDays = Saturday | Sunday,
        Day = WeekDays | WeekendDays
    }
    public enum HourlyFrecuencysEnum
    {
        None,
        OccursOne,
        OccursEvery
    }
    public enum MonthlyFrecuencyEnum
    {
        FixedDay,
        DayPosition
    }
    public enum DayPositionEnum 
    {
        First,
        Second,
        Third,
        Fourth,
        Last
    }
    public struct OutData
    {
        public DateTime OcurrenceDate;
        public string NextExecutionTime;
        public string Description;
    }



    public class SchedulerException : Exception
    {
        public SchedulerException(String mensaje) : base(mensaje) { }
    }
    public class SchedulerManager
    {
        public RecurrenceTypesEnum Type;
        public int Periodicity;
        public DateTime StartDate;
        public DateTime EndDate;
        public DateTime CurrentDate;
        public int WeekDays = 0;

        // Hourly Frecuency;
        public HourlyFrecuencysEnum HourlyFrecuency;
        public DateTime HourlyOccursAt;
        public int HourlyOccursEvery;
        public TimeIntervalEnum HourlyOccursEveryInterval;
        public DateTime HourlyStartAt;
        public DateTime HourlyEndAt;
        // Monthly Frecuency;
        public MonthlyFrecuencyEnum MonthlyFrecuency;
        public int MonthlyFixedDay;
        public DayPositionEnum MonthlyDayPosition;




        public OutData NextOcurrence()
        {
            InputValidations();

            
            OutData salida = new OutData();
           
            switch (Type)
            {
                case RecurrenceTypesEnum.Once:
                    salida.OcurrenceDate = OneOcurrence();
                    break;
                case RecurrenceTypesEnum.Daily:
                    salida.OcurrenceDate  = DailyRecurrence(this.CurrentDate ,this.Periodicity);
                    break;
                case RecurrenceTypesEnum.Weekly:
                    salida.OcurrenceDate = WeeklyRecurrence();
                    break;
                case RecurrenceTypesEnum.Monthly:
                    salida.OcurrenceDate = MonthlyRecurrence();
                    break;

            }


            if (this.EndDate != DateTime.MinValue && salida.OcurrenceDate > EndDate)
                throw new SchedulerException("La ocurrencia calculada supera la fecha limite establecida.");

            salida.NextExecutionTime = salida.OcurrenceDate.ToString("dd/MM/yyyy HH:mm");
            salida.Description = DescriptionMount(salida);

            return salida;
        }
        private DateTime OneOcurrence()
        {
            DateTime salida;

            salida = CurrentDate;

            return salida;
        }
        private DateTime HourlyRecurrence(DateTime CurrentDate,bool First)
        {
            DateTime WorkDateTime;

            WorkDateTime = CurrentDate;
            switch (this.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    if (new DateTime(1001, 1, 1, WorkDateTime.Hour, WorkDateTime.Minute, 0) < new DateTime(1001, 1, 1, this.HourlyOccursAt.Hour, this.HourlyOccursAt.Minute, 0))
                        WorkDateTime = WorkDateTime.SetTime(this.HourlyOccursAt.Hour, this.HourlyOccursAt.Minute, this.HourlyOccursAt.Second);
                    else
                        WorkDateTime = DateTime.MinValue;
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    if (First)
                    {
                        if (WorkDateTime.Hour == 0 && this.HourlyStartAt.Hour == 0)
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

        private DateTime DailyRecurrence(DateTime CurrentDate,int DailyPeriodicity)
        {
            DateTime Salida;
            DateTime WorkDateTime;

            WorkDateTime = CurrentDate;
            switch (this.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    Salida = HourlyRecurrence(WorkDateTime,false);
                    if (Salida != DateTime.MinValue)
                        WorkDateTime = Salida;
                    else
                    {
                        WorkDateTime = CurrentDate.AddDays(DailyPeriodicity);
                        WorkDateTime = WorkDateTime.SetTime(this.HourlyOccursAt.Hour, this.HourlyOccursAt.Minute, this.HourlyOccursAt.Second);
                    }
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    Salida = HourlyRecurrence(WorkDateTime,false);
                    if (Salida != DateTime.MinValue)
                        WorkDateTime = Salida;
                    else
                    {
                        WorkDateTime = WorkDateTime.AddDays(Periodicity);
                        WorkDateTime = WorkDateTime.SetTime(this.HourlyStartAt.Hour, 0);
                    }
                    break;
                default:
                    WorkDateTime = WorkDateTime.AddDays(Periodicity);
                    break;
            }
            return WorkDateTime;
        }
        private DateTime WeeklyRecurrence()
        {
            DateTime WorkDateTime;
            bool Processed = false;
            int CurrentWeekNumber;


            WorkDateTime = this.CurrentDate;
            CurrentWeekNumber = GetWeekNumber(WorkDateTime);
            while (Processed == false)
            {
                if (DayOfWeekIncluded(WorkDateTime))
                {
                    if (this.HourlyFrecuency != HourlyFrecuencysEnum.None)
                    {
                        var tmp_datetime = this.HourlyRecurrence(WorkDateTime,WorkDateTime != this.CurrentDate );
                        if (tmp_datetime != DateTime.MinValue)
                        {
                            WorkDateTime = tmp_datetime;
                            break;
                        }

                    }
                    else
                        if (WorkDateTime != this.CurrentDate)
                            break;
                }
                WorkDateTime = WorkDateTime.AddDays(1);
                WorkDateTime = WorkDateTime.SetTime(0, 0, 0);

                if (CurrentWeekNumber != GetWeekNumber(WorkDateTime))
                {
                    WorkDateTime = WorkDateTime.AddDays(7 * (-1));   // Ajusta al lunes de la semana actual
                    WorkDateTime = WorkDateTime.AddDays(7 * this.Periodicity);  // Avanza las semanas indicadas
                    CurrentWeekNumber = GetWeekNumber(WorkDateTime);
                }
            }

            return WorkDateTime;
        }
        private DateTime MonthlyRecurrence()
        {
            DateTime WorkDateTime;
            DateTime datetime_tmp;

            WorkDateTime = this.CurrentDate;

            datetime_tmp = DateTime.MinValue;
            while (true)
            {
                if (this.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay)
                    datetime_tmp = GetDateByDayFixed(this.MonthlyFixedDay , WorkDateTime);
                else
                    datetime_tmp = GetDateByDayPosition(this.MonthlyDayPosition, this.WeekDays, WorkDateTime);
                if (WorkDateTime <= datetime_tmp)
                {
                    WorkDateTime = datetime_tmp;
                    if (WorkDateTime != this.CurrentDate || this.HourlyFrecuency != HourlyFrecuencysEnum.None)
                    {
                        if (this.HourlyFrecuency != HourlyFrecuencysEnum.None)
                        {
                            var tmp_datetime = this.HourlyRecurrence(WorkDateTime, WorkDateTime != this.CurrentDate);
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
                WorkDateTime = new DateTime(CurrentDate.Year, CurrentDate.Month, 1, 0, 0, 0);
                WorkDateTime = WorkDateTime.AddMonths(this.Periodicity);
            }
            
            return WorkDateTime;
        }
        private DateTime GetDateByDayFixed(int FixedDay,DateTime BeginDate)
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
        private DateTime GetDateByDayPosition(DayPositionEnum search_position, int weekDays,DateTime WorkDateTime)
        {
            int Position = 99;
            DateTime LastDayFound = DateTime.MinValue ;
            int ActualMonth = WorkDateTime.Month;
            int Contador = 0;

            if (search_position != DayPositionEnum.Last)
                Position = (int)search_position + 1;

            WorkDateTime = new DateTime(WorkDateTime.Year, WorkDateTime.Month, 1,WorkDateTime.Hour,WorkDateTime.Minute,WorkDateTime.Second);

            // Procesa todos los días del mes de la fecha recibida
            while (ActualMonth == WorkDateTime.Month)
            {
                // Si el dia actual es lunes comprueba si se esta buscando el lunes, Dia de la semana de trabajo o cualquier dia. Lo mismo para el resto de los dias de la semana (Salvo sabados y domingos)
                if ((WorkDateTime.DayOfWeek == DayOfWeek.Monday && (weekDays == (int)WeekDaysEnum.Monday || weekDays == (int)WeekDaysEnum.WeekDays || weekDays == (int)WeekDaysEnum.Day))||
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

            if ((WorkDateTime.DayOfWeek == DayOfWeek.Monday && (this.WeekDays & (int)WeekDaysEnum.Monday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Tuesday && (this.WeekDays & (int)WeekDaysEnum.Tuesday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Wednesday && (this.WeekDays & (int)WeekDaysEnum.Wednesday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Thursday && (this.WeekDays & (int)WeekDaysEnum.Thursday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Friday && (this.WeekDays & (int)WeekDaysEnum.Friday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Saturday && (this.WeekDays & (int)WeekDaysEnum.Saturday) != 0) ||
            (WorkDateTime.DayOfWeek == DayOfWeek.Sunday && (this.WeekDays & (int)WeekDaysEnum.Sunday) != 0))
            {
                Result = true;
            }

            return Result;
        }
        private void InputValidations()
        {
            // Comprueba que se pase la fecha actual para el calculo de la siguiente ocurrencia
            if (CurrentDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

            if (StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if ((this.Type == RecurrenceTypesEnum.Daily || this.Type == RecurrenceTypesEnum.Weekly || this.Type == RecurrenceTypesEnum.Monthly) && this.Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");


            // Se comprueba el currentdate no sea inferior a la fecha de inicio
            if (StartDate != DateTime.MinValue && CurrentDate < StartDate)
                throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");

            // Comprueba que se han indicado algun dia de la semana en la recurrencia semanal
            if (this.Type == RecurrenceTypesEnum.Weekly && this.WeekDays == 0)
                throw new SchedulerException("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.");

            if (this.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery)
            {
                // Si se indica periodicidad horaria se comprueba que se han indicado la hora de limite desde-hasta
                if (this.HourlyStartAt == DateTime.MinValue && this.HourlyEndAt == DateTime.MinValue)
                    throw new SchedulerException("Debe indicarse las horas limite cuando el modo horario es periodico.");
                // Se comprueba que la hora desde no es superior a la hora hasta en modo hourly
                if (this.HourlyStartAt >= this.HourlyEndAt)
                    throw new SchedulerException("La hora 'desde' debe ser inferior a la hora 'hasta'.");
            }

            // Si estamos en modo hourly periodico se comprueba que se envia la periodicidad de horas
            if (this.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery && this.HourlyOccursEvery <= 0)
                throw new SchedulerException("Debe indicarse un valor para la periodicidad horaria.");

            // Comprobaciones recurrencia Mensual
            if (this.Type == RecurrenceTypesEnum.Monthly)
            {
                if (this.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay  &&  (this.MonthlyFixedDay <= 0 || this.MonthlyFixedDay > 31))
                    throw new SchedulerException("El día debe del mes estar comprendido entre 1 y 31.");
            }
        }
        private int GetWeekNumber(DateTime ActualDate)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(ActualDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        private string DescriptionMount(OutData salida)
        {
            string Description = "";

            switch(this.Type)
            {
                case RecurrenceTypesEnum.Once:
                    Description = "Occurs One";
                    break;
                case RecurrenceTypesEnum.Daily:
                    Description = "Occurs every " + this.Periodicity + " day(s)";
                    break;
                case RecurrenceTypesEnum.Weekly:
                    Description = "Occurs every " + this.Periodicity + " week(s) on "+StringDays();
                    break;
                case RecurrenceTypesEnum.Monthly:
                    if (this.MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay)
                    {
                        Description = "Occurs on day " + this.MonthlyFixedDay;
                    } else
                    {
                        Description = "Occurs the " + Enum.GetName(typeof(DayPositionEnum), this.MonthlyDayPosition).ToLower() + " " + Enum.GetName(typeof(WeekDaysEnum), (WeekDaysEnum)this.WeekDays).ToLower();
                    }
                    Description += " of every " + this.Periodicity + " month(s)";
                    break;

            }
            switch (this.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    Description += " at " + this.HourlyOccursAt.ToString("HH:mm");
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    Description += ". Every " + this.HourlyOccursEvery + " hour(s) between " + this.HourlyStartAt.ToString("HH:mm") + " and " + this.HourlyEndAt.ToString("HH:mm");
                    break;
                default:
                    break;
            }
            Description += ". Schedule will be used on " + salida.OcurrenceDate.ToString("dd/MM/yyyy") +
                            " at " + salida.OcurrenceDate.ToString("HH:mm") + " starting on " + this.StartDate.ToString("dd/MM/yyyy");

            return Description;
        }
        private string StringDays()
        {
            string daystext = "";

            if ((this.WeekDays & (int)WeekDaysEnum.Monday) != 0)
            {
                daystext = "Monday";
            }
            if ((this.WeekDays & (int)WeekDaysEnum.Tuesday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "Tuesday";
            }
            if ((this.WeekDays & (int)WeekDaysEnum.Wednesday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "Wednesday";
            }
            if ((this.WeekDays & (int)WeekDaysEnum.Thursday) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "Thursday";
            }
            if ((this.WeekDays & (int)WeekDaysEnum.Friday ) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "Friday";
            }
            if ((this.WeekDays & (int)WeekDaysEnum.Saturday ) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "Saturday";
            }
            if ((this.WeekDays & (int)WeekDaysEnum.Sunday ) != 0)
            {
                if (daystext != "")
                    daystext += ", ";
                daystext += "Sunday";
            }


            return daystext;
        }

        public int NextHourInterval(int Hour)
        {
            int x;
            int NextHour = -1;

            x = this.HourlyStartAt.Hour;
            while (x <= this.HourlyEndAt.Hour)
            {
                if (Hour < x)
                {
                    NextHour = x;
                    break;
                }
                x += this.HourlyOccursEvery;

            }
            return NextHour;
        }
    }
}

