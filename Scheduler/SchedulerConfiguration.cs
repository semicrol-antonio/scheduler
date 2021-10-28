using System;
using System.Collections;
using System.Collections.Generic;

namespace Scheduler
{
    public class OutData
    {
        public DateTime OcurrenceDate;
        public string NextExecutionTime;
        public string Description;
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
            set
            {
                language = value;
                LanguageManager = new SchedulerLanguage(language);
            }
        }
    }
}

