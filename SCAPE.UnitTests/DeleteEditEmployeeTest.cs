using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using SCAPE.Application.DTOs;
using SCAPE.Application.Interfaces;
using SCAPE.Application.Services;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.FaceRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SCAPE.UnitTests
{
    public class DeleteEditEmployeeTest

    {
        private readonly IEmployeeService _employeeService;


        public DeleteEditEmployeeTest(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Fact]
        public async Task EditEmployeeWithNewName()
        {
            //Employee´s documentId is "9999" and new name is Carlos
            Employee employeeEdit = new Employee();
            employeeEdit.DocumentId = "9999";
            employeeEdit.FirstName = "Carlos";
            employeeEdit.LastName = "Garcia";
            employeeEdit.Email = "martuchis@gmail.com";
            employeeEdit.Sex = "M";
            employeeEdit.DateBirth = new DateTime(2021, 07, 04);

            Assert.True(await _employeeService.editEmployee("9999", employeeEdit));


        }

        [Fact]
        public async Task EditAllFieldsEmployeee()
        {
            //Employee´s documentId is "9999" and new fields are Name: Marta, LastName: Jaramillo, Sex: F and dateBirth : 12 - 12 - 1987

            Employee employeeEdit = new Employee();
            employeeEdit.DocumentId = "9999";
            employeeEdit.FirstName = "Marta";
            employeeEdit.LastName = "Jaramillo";
            employeeEdit.Email = "falcaito@gmail.com";
            employeeEdit.Sex = "F";
            employeeEdit.DateBirth = new DateTime(1987, 12, 12);

            Assert.True(await _employeeService.editEmployee("9999", employeeEdit));


        }

        [Fact]
        public async Task EditEmployeeeWithIncorrectDocumentId()
        {
            //Edit An employee with an incorrect documentId 

            Employee employeeEdit = new Employee();
            employeeEdit.DocumentId = "9999";
            employeeEdit.FirstName = "Marta";
            employeeEdit.LastName = "Jaramillo";
            employeeEdit.Email = "falcaito@gmail.com";
            employeeEdit.Sex = "F";
            employeeEdit.DateBirth = new DateTime(1987, 12, 12);

            EmployeeException exception = await Assert.ThrowsAsync<EmployeeException>(
                     () => _employeeService.editEmployee("485215", employeeEdit)
                  );
            Assert.Equal("There was an error editing the employee ", exception.Message);

        }
        [Fact]
        public async Task EditEmployeeeWithEmptyFields()
        {
            //Edit An employee with an incorrect documentId 

            Employee employeeEdit = new Employee();
            employeeEdit.DocumentId = "9999";
            employeeEdit.FirstName = "Marta";
            employeeEdit.Email = "falcaito@gmail.com";
            employeeEdit.Sex = "F";
            employeeEdit.DateBirth = new DateTime(1987, 12, 12);

            DbUpdateException exception = await Assert.ThrowsAsync<DbUpdateException>(
                     () => _employeeService.editEmployee("9999", employeeEdit)
                  );
            Assert.Equal("An error occurred while updating the entries. See the inner exception for details.", exception.Message);
        }
        [Fact]
        public async Task RecoverFalcaoGarcia()
        {
            //This test is to recover an employee 

            Employee employeeEdit = new Employee();
            employeeEdit.DocumentId = "9999";
            employeeEdit.FirstName = "Radamel Falcao";
            employeeEdit.LastName = "Garcia";
            employeeEdit.Email = "falcaito@gmail.com";
            employeeEdit.Sex = "M";
            employeeEdit.DateBirth = new DateTime(2021, 04, 07);

            Assert.True(await _employeeService.editEmployee("9999", employeeEdit));

        }

        [Fact]
        public async Task DeleteEmployeeeByDocumentId()
        {
            //Delete employee by documentId, first create a test employee with DocumentId 252525 FirstName: User, LastName: Delete,
            //Email:deleteuser@paila.com,  sex : H and DateBirth : 2021,11,10
            
            Employee newEmployee = new Employee();

            newEmployee.DocumentId = "252525";
            newEmployee.FirstName = "User";
            newEmployee.LastName = "Delete";
            newEmployee.Email = "deleteuser@paila.com";
            newEmployee.Sex = "H";
            newEmployee.DateBirth = new DateTime(2021, 11, 10);

            await _employeeService.insertEmployee(newEmployee);

            Assert.Equal(await _employeeService.deleteEmployee("252525"), "deleteuser@paila.com");



        }



    }
}