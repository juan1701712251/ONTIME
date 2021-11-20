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
    public class Employee_WorkPlaceRepository : IEmployee_WorkPlaceRepository
    {
        private readonly SCAPEDBContext _context;

        public Employee_WorkPlaceRepository(SCAPEDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add Employee WorkPlace
        /// </summary>
        /// <param name="newEmployeeWorkPlace">new EmployeeWorkPlace</param>
        /// <returns>A successful call returns true</returns>
        public async Task<bool> addWorkPlaceByEmployee(EmployeeWorkPlace newEmployeeWorkPlace)
        {
            try
            {
                _context.EmployeeWorkPlace.Add(newEmployeeWorkPlace);
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                return false;
            }
            return true;
        }

        public async Task<EmployeeWorkPlace> findEmployeeWorkPlace(int workPlaceId, int idEmployee)
        {
            return await _context.EmployeeWorkPlace.FirstOrDefaultAsync(ew => ew.IdEmployee == idEmployee && ew.IdWorkPlace == workPlaceId);
        }

        /// <summary>
        /// Get employees with Image by workplace id(SCAPEDB in this case)
        /// </summary>
        /// <returns>>A successful call returns a WorkPlace</returns>
        public async Task<WorkPlace> getEmployeesWithImageByWorkPlace(int workPlaceId)
        {
            //Verify that Workplace Exist

            WorkPlace wp = await _context.WorkPlace.Where(w => w.Id == workPlaceId)
                                                        .Select(w => new WorkPlace{
                                                            Id = w.Id,
                                                            Name = w.Name,
                                                            Address = w.Address,
                                                            LatitudePosition = w.LatitudePosition,
                                                            LongitudePosition = w.LongitudePosition,
                                                            Description = w.Description,
                                                            EmployeeWorkPlace = w.EmployeeWorkPlace.Select(e => new EmployeeWorkPlace
                                                                                                            {
                                                                                                                IdEmployee = e.IdEmployee,
                                                                                                                IdWorkPlace = e.IdWorkPlace,
                                                                                                                StartJobDate = e.StartJobDate,
                                                                                                                EndJobDate = e.EndJobDate,
                                                                                                                Employee = new Employee
                                                                                                                {
                                                                                                                    Id = e.Employee.Id,
                                                                                                                    FirstName = e.Employee.FirstName,
                                                                                                                    LastName = e.Employee.LastName,
                                                                                                                    DocumentId = e.Employee.DocumentId,
                                                                                                                    Email = e.Employee.Email,
                                                                                                                    Image = e.Employee.Image
                                                                                                                }
                                                                                                            }).ToList()
                                                        }).FirstOrDefaultAsync();

            if (wp == null) return null;

            return wp;

            /*
            return await _context.EmployeeWorkPlace.Where(e => e.IdWorkPlace == workPlaceId)
                                                    .Select(e => new EmployeeWorkPlace
                                                    {
                                                        IdEmployee = e.IdEmployee,
                                                        IdWorkPlace = e.IdWorkPlace,
                                                        StartJobDate = e.StartJobDate,
                                                        EndJobDate = e.EndJobDate,
                                                        Schedule = e.Schedule,
                                                        Employee = new Employee { Id = e.Employee.Id,
                                                                                    FirstName = e.Employee.FirstName, 
                                                                                    LastName = e.Employee.LastName, 
                                                                                    DocumentId = e.Employee.DocumentId,
                                                                                    Email = e.Employee.Email,
                                                                                    Image = e.Employee.Image },
                                                        WorkPlace = new WorkPlace
                                                                                {
                                                                                    Id = e.WorkPlace.Id,
                                                                                    Name = e.WorkPlace.Name,
                                                                                    Address = e.WorkPlace.Address,
                                                                                    LatitudePosition = e.WorkPlace.LatitudePosition,
                                                                                    LongitudePosition = e.WorkPlace.LongitudePosition,
                                                                                    Description = e.WorkPlace.Description
                                                                                }
                                                    }).ToListAsync();
            */


        }

        public async Task<bool> update(EmployeeWorkPlace employeeWorkPlace)
        {
            try
            {
                _context.EmployeeWorkPlace.Update(employeeWorkPlace);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}


