using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCAPE.Application.DTOs
{
    public  class WorkPlaceWithEmployeesDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string LatitudePosition { get; set; }
        public string LongitudePosition { get; set; }
        public string Description { get; set; }
        public virtual ICollection<EmployeeWorkPlaceDTO> EmployeeWorkPlace { get; set; }

    }
}
