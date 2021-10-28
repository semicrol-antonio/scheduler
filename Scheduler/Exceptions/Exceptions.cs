using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerException : Exception
    {
        public SchedulerException(String mensaje) : base(mensaje) { }
    }
}
