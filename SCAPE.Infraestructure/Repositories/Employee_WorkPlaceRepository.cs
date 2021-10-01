﻿using Microsoft.Data.SqlClient;
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
        /// Get employees with Image by workplace id(SCAPEDB in this case)
        /// </summary>
        /// <returns>>A successful call returns a Employee's List</returns>
        public async Task<List<EmployeeWorkPlace>> getEmployeesWithImageByWorkPlace(int workPlaceId)
        {
            return await _context.EmployeeWorkPlace.Where(e => e.IdWorkPlace == workPlaceId)
                                                    .Select(e => new EmployeeWorkPlace
                                                    {
                                                        IdWorkPlace = e.IdWorkPlace,
                                                        StartJobDate = e.StartJobDate,
                                                        EndJobDate = e.EndJobDate,
                                                        Schedule = e.Schedule,
                                                        Employee = new Employee { Id = e.Employee.Id,
                                                                                    FirstName = e.Employee.FirstName, 
                                                                                    LastName = e.Employee.LastName, 
                                                                                    DocumentId = e.Employee.DocumentId,
                                                                                    Email = e.Employee.Email,
                                                                                    Image = e.Employee.Image }
                                                    }).ToListAsync();
        }
    }
}

