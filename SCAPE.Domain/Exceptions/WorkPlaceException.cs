using System;
using System.Collections.Generic;
using System.Text;

namespace SCAPE.Domain.Exceptions
{
    public class WorkPlaceException : Exception
    {
        public WorkPlaceException() { }

        public WorkPlaceException(string message) : base(message) { }
    }
}
