using System;
using System.Collections;
using System.Collections.Generic;

namespace Scheduler
{
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

        public OutData NextOcurrence()
        {
            var Gestor = new SchedulerManager(this);

            return Gestor.NextOcurrence();
        }
        public List<OutData> CalculateSerieByOcurrenceNumber(int num_ocurrences)
        {
            var salida = new List<OutData>();
            OutData ocurrencia;
            var Gestor = new SchedulerManager(this);
            this.EndDate = DateTime.MinValue;

            for (int x = 0; x < num_ocurrences; x++)
            {
                ocurrencia = Gestor.NextOcurrence();
                salida.Add(ocurrencia);
                this.CurrentDate = ocurrencia.OcurrenceDate;
            }

            return salida;

        }
        public List<OutData> CalculateSerieByLimitsDate()
        {
            var salida = new List<OutData>();
            OutData ocurrencia;

            if (StartDate == DateTime.MinValue || EndDate == DateTime .MinValue)
                throw new SchedulerException("Para ejecutar la serie deben indicarse la fecha de inicio y fin de ejecución.");

            var Gestor = new SchedulerManager(this);

            while (true)
            {
                try
                {
                    ocurrencia = Gestor.NextOcurrence();
                    salida.Add(ocurrencia);
                    this.CurrentDate = ocurrencia.OcurrenceDate;

                }
                catch (SchedulerException ex)
                {
                    break;
                }
            }
            return salida;

        }
    }
}

