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
    public class WorkPlaceRepository : IWorkPlaceRepository
    {
        private readonly SCAPEDBContext _context;

        public WorkPlaceRepository(SCAPEDBContext context)
        {
            _context = context;
        }

        public async Task<int> insertWorkPlace(WorkPlace workPlace)
        {
            try
            {
                _context.WorkPlace.Add(workPlace);
                await _context.SaveChangesAsync();
                return workPlace.Id;
            }
            catch (Exception ex) 
            {
                return -1;
            }
        }
    }
}


