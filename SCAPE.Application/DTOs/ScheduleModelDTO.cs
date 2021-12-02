using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.Application.DTOs
{
    public class ScheduleModelDTO
    {
        [Range(1,7)]
        public int dayOfWeek { get; set; }
        [Range(0, 1339)]
        public int startMinute { get; set; }
        [Range(0,1339)]
        public int endMinute { get; set; }
    }
}
