using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler

{
    public enum RecurrenceTypesEnum
    {
        Once,
        Daily,
        Weekly
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
        Saturday = 64
    }
    public enum HourlyFrecuencysEnum
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
                    salida.OcurrenceDate  = DailyRecurrence(this.Periodicity);
                    break;
                case RecurrenceTypesEnum.Weekly:
                    salida.OcurrenceDate = WeeklyRecurrence();
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
        private DateTime DailyRecurrence(int DailyPeriodicity)
        {
            DateTime salida;
            DateTime HoraDesde = DateTime.MinValue;
            DateTime HoraHasta = DateTime.MinValue;
            DateTime WorkDateTime;
            bool Processed;
            DateTime ActualDate;

            WorkDateTime = this.CurrentDate;
            switch (this.HourlyFrecuency)
            {
                case HourlyFrecuencysEnum.OccursOne:
                    WorkDateTime = CurrentDate.AddDays(DailyPeriodicity);
                    WorkDateTime = WorkDateTime.SetTime(this.HourlyOccursAt.Hour, this.HourlyOccursAt.Minute, this.HourlyOccursAt.Second);
                    break;
                case HourlyFrecuencysEnum.OccursEvery:
                    // Establece limites horarios en caso de llegar vacios se estable el limite entre las 0 horas y las 23:59
                    HoraDesde = HoraDesde.SetTime(this.HourlyStartAt.Hour, this.HourlyStartAt.Minute);
                    HoraHasta = HoraHasta.SetTime(this.HourlyEndAt.Hour, this.HourlyEndAt.Minute);

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
                                if (WorkDateTime.Hour == HoraDesde.Hour)
                                    Processed = true;
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

            return salida;
        }
        private DateTime WeeklyRecurrence()
        {
            DateTime salida;
            DateTime WorkDateTime;
            bool Processed = false;
            int CurrentWeekNumber;
            DateTime salidatmp;
            bool WeekChanged = false;


            WorkDateTime = this.CurrentDate;
            CurrentWeekNumber = GetWeekNumber(WorkDateTime);
            while (Processed == false)
            {
                if (HourlyFrecuency != HourlyFrecuencysEnum.OccursEvery && WeekChanged == false)
                {
                    WorkDateTime = WorkDateTime.AddDays(1);
                    WorkDateTime = WorkDateTime.SetTime(0, 0, 0);
                }
                WeekChanged = false;
                if (CurrentWeekNumber == GetWeekNumber(WorkDateTime))
                {

                    if (DayOfWeekIncluded(WorkDateTime))
                    {
                        if (this.HourlyFrecuency != HourlyFrecuencysEnum.None)
                        {
                            // Enlaza con la frecuencia diaria 
                            this.CurrentDate = WorkDateTime;
                            salidatmp = DailyRecurrence(0);
                            if (salidatmp.ToString("dd/MM/yyyy") == WorkDateTime.ToString("dd/MM/yyyy"))
                                return salidatmp;
                        }
                        else
                            Processed = true;
                    }
                    if (Processed == false && this.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery )
                    {
                        WorkDateTime = WorkDateTime.AddDays(1);
                        WorkDateTime = WorkDateTime.SetTime(0, 0, 0);
                    }
                } 
                else
                // Si ha cambiado la semana se avanza el número de semanas indicado en Periodicity
                {
                    if (Processed == false)
                    {
                        WorkDateTime = WorkDateTime.AddDays(7 * (-1));   // Ajusta al lunes de la semana actual
                        WorkDateTime = WorkDateTime.AddDays(7 * this.Periodicity);  // Avanza las semanas indicadas
                        CurrentWeekNumber = GetWeekNumber(WorkDateTime);
                        WeekChanged = true; 
                    }
                }
            }


            salida = WorkDateTime;
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
            // Comprueba que se pase la fecha actual para el calculo de la siguiente ocurrencia
            if (CurrentDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

            if (StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if ((this.Type == RecurrenceTypesEnum.Daily || this.Type == RecurrenceTypesEnum.Weekly) && this.Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");


            // Se comprueba el currentdate no sea inferior a la fecha de inicio
            if (StartDate != DateTime.MinValue && CurrentDate < StartDate)
                throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");

            // Se comprueba que la hora desde no es superior a la hora hasta en modo hourly
            if (this.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery && this.HourlyStartAt > this.HourlyEndAt)
                throw new SchedulerException("La hora 'desde' debe ser inferior a la hora 'hasta'.");

            // Comprueba que se han indicado algun dia de la semana en la recurrencia semanal
            if (this.Type == RecurrenceTypesEnum.Weekly && this.WeekDays == 0)
                throw new SchedulerException("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.");

            // Si se indica periodicidad horaria se comprueba que se han indicado la hora de limite desde-hasta
            if (this.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery && this.HourlyStartAt == DateTime.MinValue && this.HourlyEndAt == DateTime.MinValue)
                throw new SchedulerException("Debe indicarse las horas limite cuando el modo horario es periodico.");

            // Si estamos en modo hourly periodico se comprueba que se envia la periodicidad de horas
            if (this.HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery && this.HourlyOccursEvery <= 0)
                throw new SchedulerException("Debe indicarse un valor para la periodicidad horaria.");



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
                    Description = "Occurs One. Schedule will be used on " + salida.OcurrenceDate.ToString("dd/MM/yyyy") +
                        " at " + salida.OcurrenceDate.ToString("HH:mm") + " starting on " + this.StartDate.ToString("dd/MM/yyyy");

                    break;
                case RecurrenceTypesEnum.Daily:
                    Description = "Occurs every " + this.Periodicity + " day(s)";
                    switch(this.HourlyFrecuency)
                    {
                        case HourlyFrecuencysEnum.OccursOne:
                            Description += " at " + this.HourlyOccursAt.ToString("HH:mm");
                            break;
                        case HourlyFrecuencysEnum.OccursEvery:
                            Description += ". Every " + this.HourlyOccursEvery   + " hour(s) between " + this.HourlyStartAt.ToString("HH:mm") + " and " + this.HourlyEndAt.ToString("HH:mm");
                            break;
                        default:
                            break;
                    }
                    Description += ". Schedule will be used on " + salida.OcurrenceDate.ToString("dd/MM/yyyy") +
                                 " at " + salida.OcurrenceDate.ToString("HH:mm") + " starting on " + this.StartDate.ToString("dd/MM/yyyy");
                    break;
                case RecurrenceTypesEnum.Weekly:
                    Description = "Occurs every " + this.Periodicity + " week(s) on "+StringDays();
                    switch (this.HourlyFrecuency)
                    {
                        case HourlyFrecuencysEnum.OccursOne:
                            Description += " at " + this.HourlyOccursAt.ToString("HH:mm");
                            break;
                        case HourlyFrecuencysEnum.OccursEvery:
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

