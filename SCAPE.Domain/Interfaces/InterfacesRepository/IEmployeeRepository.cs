using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> insertEmployee(Employee employee);
        Task saveImageEmployee(EmployeeImage image);
        Task<Employee> findEmployee(string documentId);
        Task<bool> existEmployeeByEmail(string email);
        Task<Employee> findEmployeeByPersistedFaceId(string persistedFaceId);
        Task<List<Employee>> getEmployees();
        Task<bool> editEmployee(string documentIdOLD, Employee employee);
        Task<string> deleteEmployee(string documentId);
    }
}
