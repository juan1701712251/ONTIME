using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.Infraestructure.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly SCAPEDBContext _context;

        public EmployerRepository(SCAPEDBContext context)
        {
            _context = context;
        }

        public async Task<Employer> findEmployerByEmail(string email)
        {
            Employer employer = await _context.Employer.FirstOrDefaultAsync(i => i.Email == email);
            return employer;
        }
    }
}
