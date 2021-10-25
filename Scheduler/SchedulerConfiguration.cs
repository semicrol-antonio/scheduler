using System;
using System.Collections;
using System.Collections.Generic;

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
    public enum SupportedLanguagesEnum
    {
        es_ES,
        en_GB,
        en_US
    }
    public enum RangeEnum
    {
        OcurrenceCount,
        EndByDate
    }
    public class OutData
    {
        public DateTime OcurrenceDate;
        public string NextExecutionTime;
        public string Description;
    }
    public class SchedulerException : Exception
    {
        public SchedulerException(String mensaje) : base(mensaje) { }
    }

    public class SchedulerConfiguration
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
        // Range
        public RangeEnum Range;
        public int OcurrenceCount;



        private SupportedLanguagesEnum language;
        internal SchedulerLanguage LanguageManager = new SchedulerLanguage(SupportedLanguagesEnum.es_ES);

        public SupportedLanguagesEnum Language
        {
            get { return language; }
            set
            {
                language = value;
                LanguageManager = new SchedulerLanguage(language);
            }
        }

        public List<OutData> NextOcurrence()
        {
            int Ocurrences = 0;
            var salida = new List<OutData>();
            OutData ocurrencia;

            InputValidations();

            var Gestor = new SchedulerManager(this);

            while (true)
            {
                ocurrencia = Gestor.NextOcurrence();
                if (Range == RangeEnum.OcurrenceCount)
                {
                    Ocurrences++;
                    if (Ocurrences > this.OcurrenceCount)
                        break;
                } else
                {
                    if (ocurrencia.OcurrenceDate >= this.EndDate)
                        break;
                }
                salida.Add(ocurrencia);
                this.CurrentDate = ocurrencia.OcurrenceDate;
            }
            return salida;
        }
        private void InputValidations()
        {
            // si El rango es por ocurrencecount y se envia a 0 se establece 1 ejecución por defecto
            if (Range == RangeEnum.OcurrenceCount && OcurrenceCount == 0)
                OcurrenceCount = 1;

            // Si se indica rango por fecha de fin y no se indica la fecha de fin
            if (Range == RangeEnum.EndByDate && EndDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicarse la fecha de finalización cuando el rango de ejecución está delimitado por la fecha hasta.");

            // Comprueba que se pase la fecha actual para el calculo de la siguiente ocurrencia
            if (CurrentDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

            if (StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            // Se comprueba que la periodicidad no es igual o inferior a 0 cuando es de tipo diario o semanal
            if ((Type == RecurrenceTypesEnum.Daily || Type == RecurrenceTypesEnum.Weekly || Type == RecurrenceTypesEnum.Monthly) && Periodicity <= 0)
                throw new SchedulerException("La Periodicidad debe ser superior a 0.");


            // Se comprueba el currentdate no sea inferior a la fecha de inicio
            if (StartDate != DateTime.MinValue && CurrentDate < StartDate)
                throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");

            // Comprueba que se han indicado algun dia de la semana en la recurrencia semanal
            if (Type == RecurrenceTypesEnum.Weekly && WeekDays == 0)
                throw new SchedulerException("Debe indicarse algún día de la semana para la aplicación de la recurrencia semanal.");

            if (HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery)
            {
                // Si se indica periodicidad horaria se comprueba que se han indicado la hora de limite desde-hasta
                if (HourlyStartAt == DateTime.MinValue && HourlyEndAt == DateTime.MinValue)
                    throw new SchedulerException("Debe indicarse las horas limite cuando el modo horario es periodico.");
                // Se comprueba que la hora desde no es superior a la hora hasta en modo hourly
                if (HourlyStartAt >= HourlyEndAt)
                    throw new SchedulerException("La hora 'desde' debe ser inferior a la hora 'hasta'.");
            }

            // Si estamos en modo hourly periodico se comprueba que se envia la periodicidad de horas
            if (HourlyFrecuency == HourlyFrecuencysEnum.OccursEvery && HourlyOccursEvery <= 0)
                throw new SchedulerException("Debe indicarse un valor para la periodicidad horaria.");

            // Comprobaciones recurrencia Mensual
            if (Type == RecurrenceTypesEnum.Monthly)
            {
                if (MonthlyFrecuency == MonthlyFrecuencyEnum.FixedDay && (MonthlyFixedDay <= 0 || MonthlyFixedDay > 31))
                    throw new SchedulerException("El día debe del mes estar comprendido entre 1 y 31.");
            }
        }

    }
}

