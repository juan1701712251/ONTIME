using System;
using System.Collections.Generic;
using System.Text;

namespace SCAPE.Domain.Exceptions
{

    public class ScheduleException : Exception
    {
        public ScheduleException() { }
        public ScheduleException(string message) : base(message) { }

    }
}
