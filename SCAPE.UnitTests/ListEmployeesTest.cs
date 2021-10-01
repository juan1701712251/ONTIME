using Microsoft.Extensions.Configuration;
using Moq;
using SCAPE.Application.Interfaces;
using SCAPE.Application.Services;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.FaceRecognition;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SCAPE.UnitTests
{
    public class ListEmployeesTest

    {
        private readonly IEmployeeService _employeeService;


        public ListEmployeesTest(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Fact]
        public async Task ListEmployeesByEmptyWorkPlace()
        {
            //In Database workplace (0) is Empty
            List<EmployeeWorkPlace> employees = await _employeeService.getEmployeesWithImageByWorkplace(0);
            Assert.NotNull(employees);
            Assert.Equal(employees.Count,0);
        }

        [Fact]
        public async Task ListEmployeesByWorkPlace()
        {
            //In Database workplace (1) has Employees
            List<EmployeeWorkPlace> employees = await _employeeService.getEmployeesWithImageByWorkplace(1);
            Assert.NotNull(employees);
            Assert.NotEqual(employees.Count, 0);
            
            //Employee id = 1, has name "Juan"
            Assert.Equal("Juan",employees.Find(x => x.IdEmployee == 1).Employee.FirstName);
        }

    }
}