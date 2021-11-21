using SCAPE.Application.DTOs;
using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task insertEmployee(Employee employee);

        Task<bool> associateFace(string documentId, string encodeImage, string faceListId);

        Task<Employee> getEmployeeByFace(string encodeImage,string faceListId);

        Task<Employee> findEmployee(string documentId);

        Task<List<Employee>> getEmployees();

        
        Task<bool> editEmployee(string documentIdOLD, Employee employeeEdit);
        Task<string> deleteEmployee(string documentId);
        Task<bool> addWorkPlaceByEmployee(string documentId, int workPlaceId, DateTime StartJobDate, DateTime EndJobDate);
        Task<bool> addScheduleByEmployee(DataScheduleModelDTO dataScheduleModel);
        Task<List<EmployeeSchedule>> getScheduleByEmployee(string documentId, int workPlaceId);
    }
}
