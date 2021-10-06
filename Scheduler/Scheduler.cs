using System;
using System.Collections.Generic;
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

    public class SchedulerException : Exception
    {
        public SchedulerException(String mensaje) : base(mensaje) { }
    }
    public class SchedulerManager
    {
        public DateTime NextOcurrence(RecurrenceTypes Type, int Periodicity, DateTime StartDate, DateTime EndDate, DateTime CurrentDate)
        {
            DateTime OcurrenceDate = DateTime.MinValue;

            // Comprueba que se envia la fecha de inicio
            if (StartDate == DateTime.MinValue)
                throw new SchedulerException("Debe indicar la fecha limite de inicio.");

            switch (Type)
            {
                case RecurrenceTypes.Once:
                    if (CurrentDate == DateTime.MinValue)
                        throw new SchedulerException("Debe indicarse la fecha y hora de ejecución");

                    OcurrenceDate = CurrentDate;
                    break;
                case RecurrenceTypes.Daily:
                    // Comprueba que se envie una fecha actual
                    if (CurrentDate == DateTime.MinValue)
                        throw new SchedulerException("Debe indicar una fecha actual para el calculo de la siguiente ocurrencia");

                    // Si se envia la fecha de inicio y la actual es inferior 
                    if (StartDate != DateTime.MinValue && CurrentDate < StartDate)
                        throw new SchedulerException("La fecha de calculo es inferior a la fecha de inicio establecida");

                    OcurrenceDate = CurrentDate.AddDays(Periodicity);

                    if (EndDate != DateTime.MinValue && OcurrenceDate > EndDate)
                        throw new SchedulerException("La ocurrencia calculada supera la fecha limite establecido");
                    break;
                default:
                    break;
            }
            return OcurrenceDate;
        }
    }
}

