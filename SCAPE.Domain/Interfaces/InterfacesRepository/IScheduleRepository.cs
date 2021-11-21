using SCAPE.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IScheduleRepository
    {
        Task<List<EmployeeSchedule>> findSchedule(int workPlaceId, int idEmployee);
        Task<bool> deleteRange(int workPlaceId, int idEmployee);
        Task<bool> addRange(List<EmployeeSchedule> schedules);
    }
}
