using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.Infraestructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SCAPEDBContext _context;

        public EmployeeRepository(SCAPEDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Insert employee into the context (SCAPEDB in this case)
        /// </summary>
        /// <param name="employee">Employee to insert</param>
        public async Task<bool> insertEmployee(Employee employee)
        {

            try
            {
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().GetType() == typeof(SqlException))
                {
                    Int32 errorCode = ((SqlException)ex.InnerException).Number;

                    if (errorCode == 2627 || errorCode == 2601)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Find employee at the context (SCAPEDB in this case)
        /// </summary>
        /// <param name="documentId">employee's documentId to find</param>
        /// <returns>>A successful call returns a Employee</returns>
        public async Task<Employee> findEmployee(string documentId)
        {
            return await _context.Employee.FirstOrDefaultAsync(e => e.DocumentId == documentId);
            
        }

        /// <summary>
        /// Find employee at the context (SCAPEDB in this case)
        /// </summary>
        /// <param name="email">employee's email to find</param>
        /// <returns>>A successful call returns true</returns>
        public async Task<bool> existEmployeeByEmail(string email)
        {
            Employee emp = await _context.Employee.FirstOrDefaultAsync(e => e.Email == email);
            return emp != null;
        }


        /// <summary>
        /// Saves the employee's image into the context (SCAPEDB in this case)
        /// </summary>
        /// <param name="image">Employee image to save</param>
        public async Task saveImageEmployee(EmployeeImage image)
        {
            _context.Image.Add(image);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Find employee by PersistedFaceId at the context (SCAPEDB in this case)
        /// </summary>
        /// <param name="persistedFaceId">persistedFaceId to find </param>
        /// <returns>A successful call returns a Employee</returns>
        public async Task<Employee> findEmployeeByPersistedFaceId(string persistedFaceId)
        {
            EmployeeImage image = await _context.Image.FirstOrDefaultAsync(i => i.PersistenceFaceId == persistedFaceId);
            
            if (image == null)
            {
                return null;
            }
            
            return await _context.Employee.FirstOrDefaultAsync(e => e.Id == image.IdEmployee);
        }

        /// <summary>
        /// Get employees  (SCAPEDB in this case)
        /// </summary>
        /// <returns>>A successful call returns a Employee's List</returns>
        public async Task<List<Employee>> getEmployees()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<bool> editEmployee(string documentIdOLD, Employee employee)
        {
            Employee employeeEdit = await _context.Employee.FirstOrDefaultAsync(e => e.DocumentId == documentIdOLD);

            if (employeeEdit != null)
            {
                employeeEdit.DocumentId = employee.DocumentId;
                employeeEdit.FirstName = employee.FirstName;
                employeeEdit.LastName = employee.LastName;
                employeeEdit.Sex = employee.Sex;
                employeeEdit.DateBirth = employee.DateBirth;
                employeeEdit.Email = employee.Email;

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<string> deleteEmployee(string documentId)
        {
            Employee employeeDelete = await _context.Employee.FirstOrDefaultAsync(e => e.DocumentId == documentId);
            string emailDelete = employeeDelete.Email;
            _context.Employee.Remove(employeeDelete);
            await _context.SaveChangesAsync();

            return emailDelete;

        }
    }
}
