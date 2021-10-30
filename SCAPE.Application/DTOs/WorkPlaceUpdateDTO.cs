using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCAPE.Application.DTOs
{
    public  class WorkPlaceUpdateDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }

    }
}
