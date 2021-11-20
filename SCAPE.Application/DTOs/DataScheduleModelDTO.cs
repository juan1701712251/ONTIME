using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.Application.DTOs
{
    public class DataScheduleModelDTO
    {
        [Required]
        public string DocumentId { get; set; }
        [RegularExpression(@"^\d*[1-9]\d*$", ErrorMessage = "workPlace ID is required")]
        public int WorkPlaceId { get; set; }
        public DateTime StartJobDate { get; set; }
        public DateTime EndJobDate { get; set; }
        public List<ScheduleModelDTO> Schedule { get; set; }
    }
}
