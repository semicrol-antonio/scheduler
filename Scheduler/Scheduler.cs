using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler

{
    public enum RecurrenceTypes
    {
        Once,
        Daily,
        Weekly,
        Monthly
    }
    public enum TimeInterval
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
        Saturday = 64
    }
    public enum HourlyFrecuencys
    {
        None,
        OccursOne,
        OccursEvery
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
        public RecurrenceTypes Type;
        public int Periodicity;
        public DateTime StartDate;
        public DateTime EndDate;
        public DateTime CurrentDate;
        public int WeekDays;

        // Hourly Frecuency;
        public HourlyFrecuencys HourlyFrecuency;
        public DateTime HourlyOccursAt;
        public int HourlyOccursEvery;
        public TimeInterval HourlyOccursEveryInterval;
        public DateTime HourlyStartAt;
        public DateTime HourlyEndAt;

        public OutData NextOcurrence()
        {
            InputValidations();

            
            OutData salida = new OutData();

            
            switch (Type)
            {
                case RecurrenceTypes.Once:
                    salida.OcurrenceDate = OneOcurrence();
                    break;
                case RecurrenceTypes.Daily:
                    salida.OcurrenceDate  = DailyRecurrence(this.Periodicity );
                    break;
                case RecurrenceTypes.Weekly:
                    salida.OcurrenceDate = WeeklyRecurrence();
                    break;
                default:
                    break;
            }

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
        private DateTime DailyRecurrence(int DailyPeriodicity)
        {
            DateTime salida;
            DateTime HoraDesde = DateTime.MinValue;
            DateTime HoraHasta = DateTime.MinValue;
            DateTime WorkDateTime;
            bool Processed;
            DateTime ActualDate;
            string HourlyDetail = "";

            WorkDateTime = this.CurrentDate;
            switch (this.HourlyFrecuency)
            {
                case HourlyFrecuencys.OccursOne:
                    WorkDateTime = CurrentDate.AddDays(DailyPeriodicity);
                    WorkDateTime = WorkDateTime.SetTime(this.HourlyOccursAt.Hour, this.HourlyOccursAt.Minute, this.HourlyOccursAt.Second);

                    HourlyDetail = ", at " + WorkDateTime.ToString("HH:mm:ss");
                    break;
                case HourlyFrecuencys.OccursEvery:
                    // Establece limites horarios en caso de llegar vacios se estable el limite entre las 0 horas y las 23:59
                    if (this.HourlyStartAt.Hour != 0)
                        HoraDesde = HoraDesde.SetTime(this.HourlyStartAt.Hour, this.HourlyStartAt.Minute);
                    else
                        HoraDesde = HoraDesde.SetTime(0, 0);

                    if (this.HourlyEndAt.Hour != 0)
                        HoraHasta = HoraHasta.SetTime(this.HourlyEndAt.Hour, this.HourlyEndAt.Minute);
                    else
                        HoraHasta = HoraHasta.SetTime(23, 59);

                    Processed = false;
                    while (Processed == false)
                    {
                        if (WorkDateTime.Hour < HoraDesde.Hour)
                        {
                            WorkDateTime = WorkDateTime.SetTime(HoraDesde.Hour);
                            Processed = true;
                        }
                        else
                        {
                            ActualDate = WorkDateTime.Date;
                            WorkDateTime = WorkDateTime.AddHours(this.HourlyOccursEvery);
                            if (ActualDate.ToString("dd/MM/YYYY") != WorkDateTime.ToString("dd/MM/YYYY"))
                            {
                                WorkDateTime = ActualDate;
                                WorkDateTime = WorkDateTime.AddDays(Periodicity);
                                WorkDateTime = WorkDateTime.SetTime(0, 0);
                            }
                            else
                            {
                                if (WorkDateTime.Hour > HoraHasta.Hour)
                                {
                                    WorkDateTime = WorkDateTime.AddDays(Periodicity);
                                    WorkDateTime = WorkDateTime.SetTime(0, 0);
                                }
                                else
                                    Processed = true;

                            }
                        }
                    }
                    break;
                default:
                    WorkDateTime = WorkDateTime.AddDays(Periodicity);
                    break;
            }


            salida = WorkDateTime;
            

            if (EndDate != DateTime.MinValue && salida > EndDate)
                throw new SchedulerException("La ocurrencia calculada supera la fecha limite establecido");

            return salida;
        }
        private DateTime WeeklyRecurrence()
        {
            DateTime salida;
            DateTime WorkDateTime;
            int WeekDay;
            bool Processed = false;
            int CurrentWeekNumber;
            DateTime salidatmp;
            bool WeekChanged = false;

            WorkDateTime = this.CurrentDate;
            CurrentWeekNumber = GetWeekNumber(WorkDateTime);


            while (Processed == false)
            {
                if (HourlyFrecuency != HourlyFrecuencys.OccursEvery && WeekChanged == false)
                {
                    WorkDateTime = WorkDateTime.AddDays(1);
                    WorkDateTime = WorkDateTime.SetTime(0, 0, 0);
                }
                WeekChanged = false;
                if (CurrentWeekNumber == GetWeekNumber(WorkDateTime))
                {

                    if (DayOfWeekIncluded(WorkDateTime))
                    {
                        if (this.HourlyFrecuency != HourlyFrecuencys.None)
                        {
                            this.CurrentDate = WorkDateTime;
                            salidatmp = DailyRecurrence(0);
                            if (salidatmp.ToString("dd/MM/yyyy") == WorkDateTime.ToString("dd/MM/yyyy"))
                                return salidatmp;
                        }
                        else
                            Processed = true;
                    }
                    if (Processed == false && this.HourlyFrecuency == HourlyFrecuencys.OccursEvery )
                    {
                        WorkDateTime = WorkDateTime.AddDays(1);
                        WorkDateTime = WorkDateTime.SetTime(0, 0, 0);
                    }
                } 
                else
                {
                    if (Processed == false)
                    {
                        WorkDateTime = WorkDateTime.AddDays(-1);
                        WeekDay = (int)WorkDateTime.DayOfWeek;
                        if (WeekDay == 0)
                            WeekDay = 6;
                        else
                            WeekDay--;

                        WorkDateTime = WorkDateTime.AddDays(WeekDay * (-1));   // Ajusta al lunes de la semana actual
                        WorkDateTime = WorkDateTime.AddDays(7 * this.Periodicity);  // Avanza las semanas indicadas
                        CurrentWeekNumber = GetWeekNumber(WorkDateTime);
                        WeekChanged = true; 
                    }
                }
            }


            salida = WorkDateTime;


            if (EndDate != DateTime.MinValue && salida > EndDate)
                throw new SchedulerException("La ocurrencia calculada supera la fecha limite establecido");

            return salida;
        }
        private bool DayOfWeekIncluded(DateTime WorkDateTime)
        {
            bool Result = false;
            if (WorkDateTime.DayOfWeek == DayOfWeek.Monday && (this.WeekDays & (int)WeekDaysEnum.Monday) != 0)
            {
                Result = true;
            }
            if (WorkDateTime.DayOfWeek == DayOfWeek.Tuesday && (this.WeekDays & (int)WeekDaysEnum.Tuesday) != 0)
            {
                Result = true;
            }
            if (WorkDateTime.DayOfWeek == DayOfWeek.Wednesday && (this.WeekDays & (int)WeekDaysEnum.Wednesday) != 0)
            {
                Result = true;
            }
            if (WorkDateTime.DayOfWeek == DayOfWeek.Thursday && (this.WeekDays & (int)WeekDaysEnum.Thursday) != 0)
            {
                Result = true;
            }
            if (WorkDateTime.DayOfWeek == DayOfWeek.Friday && (this.WeekDays & (int)WeekDaysEnum.Friday) != 0)
            {
                Result = true;
            }
            if (WorkDateTime.DayOfWeek == DayOfWeek.Saturday && (this.WeekDays & (int)WeekDaysEnum.Saturday) != 0)
            {
                Result = true;
            }
            if (WorkDateTime.DayOfWeek == DayOfWeek.Sunday && (this.WeekDays & (int)WeekDaysEnum.Sunday) != 0)
            {
                Result = true;
            }

            return Result;
        }
        private void InputValidations()
        {
            // Comprueba que se han indicado algun dia de la semana en la recurrencia semanal
            if (this.Type == RecurrenceTypes.Weekly && this.WeekDays == 0)
                throw new SchedulerException("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.");

            // Se comprueba que la hora desde no es superior a la hora hasta
            if (this.HourlyFrecuency == HourlyFrecuencys.OccursEvery && this.HourlyStartAt > this.HourlyEndAt )
                throw new SchedulerException("La hora 'desde' debe ser inferior a la hora 'hasta'.");

            // Se pone la periocidad a 1 si es menor o igual a 0
            if (this.Periodicity <= 0)
                this.Periodicity = 1;
            // Comprueba que se envia la fecha de inicio
            if (StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            switch (Type)
            {
                case RecurrenceTypes.Once:
                    if (CurrentDate == DateTime.MinValue)
                        throw new SchedulerException("Debe indicarse la fecha y hora de ejecución");

                    break;
                case RecurrenceTypes.Daily:
                    // Comprueba que se ha enviado la fecha actual
                    if (CurrentDate == DateTime.MinValue)
                        throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

                    // Si se envia la fecha de inicio y la actual es inferior 
                    if (StartDate != DateTime.MinValue && CurrentDate < StartDate)
                        throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");

                    // Comprueba que la periodicidad es mayor que 0
                    if (Periodicity <= 0)
                        throw new SchedulerException("La Periocididad debe tener un valor superior a 0");

                    break;
                case RecurrenceTypes.Weekly:
                    break;
                default:
                    break;
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
                case RecurrenceTypes.Once:
                    Description = "Occurs One. Schedule will be used on " + salida.OcurrenceDate.ToString("dd/MM/yyyy") +
                        " at " + salida.OcurrenceDate.ToString("HH:mm") + " starting on " + this.StartDate.ToString("dd/MM/yyyy");

                    break;
                case RecurrenceTypes.Daily:
                    Description = "Occurs every " + this.Periodicity + " day(s)";
                    switch(this.HourlyFrecuency)
                    {
                        case HourlyFrecuencys.OccursOne:
                            Description += " at " + this.HourlyOccursAt.ToString("HH:mm");
                            break;
                        case HourlyFrecuencys.OccursEvery:
                            Description += ". Every " + this.HourlyOccursEvery   + " hour(s) between " + this.HourlyStartAt.ToString("HH:mm") + " and " + this.HourlyEndAt.ToString("HH:mm");
                            break;
                        default:
                            break;
                    }
                    Description += ". Schedule will be used on " + salida.OcurrenceDate.ToString("dd/MM/yyyy") +
                                 " at " + salida.OcurrenceDate.ToString("HH:mm") + " starting on " + this.StartDate.ToString("dd/MM/yyyy");
                    break;
                case RecurrenceTypes.Weekly:
                    Description = "Occurs every " + this.Periodicity + " week(s) on "+StringDays();
                    switch (this.HourlyFrecuency)
                    {
                        case HourlyFrecuencys.OccursOne:
                            Description += " at " + this.HourlyOccursAt.ToString("HH:mm");
                            break;
                        case HourlyFrecuencys.OccursEvery:
                            Description += ". Every " + this.HourlyOccursEvery  + " hour(s) between " + this.HourlyStartAt.ToString("HH:mm") + " and " + this.HourlyEndAt.ToString("HH:mm");
                            break;
                        default:
                            break;
                    }
                    Description += ". Schedule will be used on " + salida.OcurrenceDate.ToString("dd/MM/yyyy") +
                                 " at " + salida.OcurrenceDate.ToString("HH:mm") + " starting on " + this.StartDate.ToString("dd/MM/yyyy");

                    break;

            }

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

    }
}

