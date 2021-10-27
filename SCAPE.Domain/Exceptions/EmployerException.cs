using System;
using System.Collections.Generic;
using System.Text;

namespace SCAPE.Domain.Exceptions
{
    public class EmployerException : Exception
    {
        public EmployerException() { }

        public EmployerException(string message) : base(message) { }
    }
}
