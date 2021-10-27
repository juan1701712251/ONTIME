using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCAPE.Application.DTOs
{
    public  class WorkPlaceDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        public string Description { get; set; }

    }
}
