using System;
using System.Collections.Generic;

namespace SCAPE.Domain.Entities
{
    public partial class Employer
    {
        public Employer()
        {
            WorkPlace = new HashSet<WorkPlace>();
        }

        public int Id { get; set; }
        public string DocumentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<WorkPlace> WorkPlace { get; set; }
    }
}
