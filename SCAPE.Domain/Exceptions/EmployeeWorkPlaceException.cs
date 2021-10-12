using System;
using System.Collections.Generic;
using System.Text;

namespace SCAPE.Domain.Exceptions
{

    public class EmployeeWorkPlaceException : Exception
    {
        public EmployeeWorkPlaceException() { }
        public EmployeeWorkPlaceException(string message) : base(message) { }

    }
}
