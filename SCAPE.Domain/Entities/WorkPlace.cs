using System;
using System.Collections.Generic;

namespace SCAPE.Domain.Entities
{
    public  class WorkPlace
    {
        public WorkPlace()
        {
            EmployeeSchedule = new HashSet<EmployeeSchedule>();
            EmployeeWorkPlace = new HashSet<EmployeeWorkPlace>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string LatitudePosition { get; set; }
        public string LongitudePosition { get; set; }
        public string FaceListId { get; set; }
        public string Description { get; set; }
        public int IdEmployer { get; set; }

        public virtual Employer IdEmployerNavigation { get; set; }
        public virtual ICollection<EmployeeSchedule> EmployeeSchedule { get; set; }
        public virtual ICollection<EmployeeWorkPlace> EmployeeWorkPlace { get; set; }
    }
}
