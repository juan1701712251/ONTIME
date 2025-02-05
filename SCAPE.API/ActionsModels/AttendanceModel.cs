﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.API.ActionsModels
{
    public class AttendanceModel
    {
        [Required]
        public string documentEmployee { get; set; }
        [Required]
        public int workPlaceId { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
    }
}
