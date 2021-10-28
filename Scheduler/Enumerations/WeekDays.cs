using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
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
}
