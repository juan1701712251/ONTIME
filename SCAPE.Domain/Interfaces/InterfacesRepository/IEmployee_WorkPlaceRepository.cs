using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IEmployee_WorkPlaceRepository
    {
        Task<WorkPlace> getEmployeesWithImageByWorkPlace(int workPlaceId);
        Task<bool> addWorkPlaceByEmployee(EmployeeWorkPlace newEmployeeWorkPlace);
        Task<EmployeeWorkPlace> findEmployeeWorkPlace(int workPlaceId, int idEmployee);
        Task<bool> update(EmployeeWorkPlace employeeWorkPlace);
        Task<bool> remove(int workPlaceId, int employeeId);
    }
}
