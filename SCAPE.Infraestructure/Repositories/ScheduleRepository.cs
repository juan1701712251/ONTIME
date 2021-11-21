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
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly SCAPEDBContext _context;

        public ScheduleRepository(SCAPEDBContext context)
        {
            _context = context;
        }

        public async Task<bool> addRange(List<EmployeeSchedule> schedules)
        {
            try
            {
                _context.AddRange(schedules);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> deleteRange(int workPlaceId, int idEmployee)
        {
            try
            {
                List<EmployeeSchedule> employeeSchedules = await this.findSchedule(workPlaceId, idEmployee);
                _context.RemoveRange(employeeSchedules);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<List<EmployeeSchedule>> findSchedule(int workPlaceId, int idEmployee)
        {
            return await _context.EmployeeSchedule.Where(s => s.IdEmployee == idEmployee && s.IdWorkPlace == workPlaceId).ToListAsync();
        }
    }
}
