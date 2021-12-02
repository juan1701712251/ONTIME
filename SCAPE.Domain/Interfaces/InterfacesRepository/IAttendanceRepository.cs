using SCAPE.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<bool> insertAttendance(Attendance attendance);
        Task<List<Attendance>> getAttendancesByEmployee(int EmployeeId);
    }
}
