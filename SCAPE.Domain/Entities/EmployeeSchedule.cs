using System;
using System.Collections.Generic;

namespace SCAPE.Domain.Entities
{
    public partial class EmployeeSchedule
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public int IdWorkPlace { get; set; }
        public int StartMinute { get; set; }
        public int EndMinute { get; set; }
        public int DayOfWeek { get; set; }

        public virtual Employee IdEmployeeNavigation { get; set; }
        public virtual WorkPlace IdWorkPlaceNavigation { get; set; }
    }
}
