using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCAPE.Application.DTOs
{
    public class EmployeeWithImageDTO
    {
        public int Id { get; set; }
        public string DocumentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DateBirth { get; set; }
        public string Sex { get; set; }

        public ICollection<EmployeeImageDTO> Image { get; set; }
    }
}
