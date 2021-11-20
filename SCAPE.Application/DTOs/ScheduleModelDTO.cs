using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.Application.DTOs
{
    public class ScheduleModelDTO
    {
        [RegularExpression(@"^\d*[1-9]\d*$", ErrorMessage = "dayOfWeek ID is required")]
        public int dayOfWeek { get; set; }
        [RegularExpression(@"^\d*[1-9]\d*$", ErrorMessage = "startMinute ID is required")]
        public int startMinute { get; set; }
        [RegularExpression(@"^\d*[1-9]\d*$", ErrorMessage = "endMinute ID is required")]
        public int endMinute { get; set; }
    }
}
