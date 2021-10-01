using System;
using System.Collections.Generic;

namespace SCAPE.Application.DTOs
{
    public  class EmployeeWorkPlaceDTO
    {
        public int IdWorkPlace { get; set; }
        public DateTime StartJobDate { get; set; }
        public DateTime EndJobDate { get; set; }
        public string Schedule { get; set; }

        public EmployeeWithImageDTO Employee { get; set; }
    }
}
