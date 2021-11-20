﻿using System;
using System.Collections.Generic;

namespace SCAPE.Domain.Entities
{
    public  class Employee
    {
        public Employee()
        {
            Attendance = new HashSet<Attendance>();
            EmployeeSchedule = new HashSet<EmployeeSchedule>();
            EmployeeWorkPlace = new HashSet<EmployeeWorkPlace>();
            Image = new HashSet<EmployeeImage>();
        }

        public int Id { get; set; }
        public string DocumentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public DateTime? DateBirth { get; set; }

        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<EmployeeSchedule> EmployeeSchedule { get; set; }
        public virtual ICollection<EmployeeWorkPlace> EmployeeWorkPlace { get; set; }
        public virtual ICollection<EmployeeImage> Image { get; set; }
    }
}
