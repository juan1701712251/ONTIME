using SCAPE.Application.DTOs;
using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<bool> addAttendance(DateTime date,string type,string documentEmployee, int workPlaceId);
        Task<List<Attendance>> getAttendancesByEmployee(string documentEmployee);
    }
}
