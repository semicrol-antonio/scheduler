using System;

namespace Scheduler.Windows
{
    public enum RecurrenceTypes
    {
        Once,
        Daily,
        Weekly,
        Monthly
    }
    public class RecurrenceManager
    {

        public DateTime NextOcurrence(RecurrenceTypes Type,int Periodicity,DateTime StartDate,DateTime EndDate,DateTime CurrentDate)
        {
            DateTime OcurrenceDate;

            OcurrenceDate = System.DateTime.Today;

            return OcurrenceDate;
        }
        public string NextOcurrenceToString(RecurrenceTypes type, int Periodicity, DateTime StartDate, DateTime EndDate, DateTime CurrentDate)
        {
            string OutString;

            OutString = "";

            return OutString;
        }

    }
}
