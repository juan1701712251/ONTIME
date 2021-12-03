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
    public class AsociateEmployeeToWorkplaceTest

    {
        private readonly IEmployeeService _employeeService;


        public AsociateEmployeeToWorkplaceTest(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Fact]
        public async Task AddSheduletoEmployee()
        {
            //Employee´s documentId is "10532525" a
            DataScheduleModelDTO dataShedule = new DataScheduleModelDTO();
            dataShedule.DocumentId = "10532525";
            dataShedule.WorkPlaceId = 69;
            dataShedule.StartJobDate = new DateTime(2021, 09, 12);
            dataShedule.EndJobDate = new DateTime(2021, 12, 12);
            List<ScheduleModelDTO> listaShedu = new List<ScheduleModelDTO>();
            ScheduleModelDTO schedule = new ScheduleModelDTO();
            schedule.dayOfWeek = 1;
            schedule.startMinute = 1;
            schedule.endMinute = 6;
            listaShedu.Add(schedule);
            dataShedule.Schedule = listaShedu;

            Assert.True(await _employeeService.addScheduleByEmployee(dataShedule));
        }
        [Fact]
        public async Task AddEmployeeToWorkPlace()
        {
            //Add Employee´s documentId is "10532525" to Workplace with id 69
            Employee newemployee = new Employee();
            newemployee.DocumentId = "10531111";
            newemployee.FirstName = "Leonidas";
            newemployee.LastName = "300";
            newemployee.Email = "leonidas@ontime.com";
            newemployee.Sex = "H";
            newemployee.DateBirth = new DateTime(1992, 09, 25);
            await _employeeService.insertEmployee(newemployee);

            DateTime start = new DateTime(2021, 09, 12);
            DateTime end = new DateTime(2021, 12, 12);
            Assert.True(await _employeeService.addWorkPlaceByEmployee("10531111", 69, start, end));
        }
        [Fact]
        public async Task NoAddSameEmployeeToWorkPlace()
        {
            //Try add the same employee pepito with document id 10532525 at workplace
            DateTime start = new DateTime(2021, 09, 12);
            DateTime end = new DateTime(2021, 12, 12);
            EmployeeWorkPlaceException exception = await Assert.ThrowsAsync<EmployeeWorkPlaceException>(
                     () => _employeeService.addWorkPlaceByEmployee("10532525", 69, start, end)
                  );
            Assert.Equal("There was an error adding the employee to workplace", exception.Message);

        }
        [Fact]
        public async Task NoAddWithIcorrectWorkplaceId()
        {
            //create Dataschedule with incorrect Workplace id 

            DataScheduleModelDTO dataShedule = new DataScheduleModelDTO();
            dataShedule.DocumentId = "10530808";
            dataShedule.WorkPlaceId = 80;
            dataShedule.StartJobDate = new DateTime(2021, 09, 12);
            dataShedule.EndJobDate = new DateTime(2021, 12, 12);

            DateTime start = new DateTime(2021, 09, 12);
            DateTime end = new DateTime(2021, 12, 12);
            EmployeeWorkPlaceException exception = await Assert.ThrowsAsync<EmployeeWorkPlaceException>(
                     () => _employeeService.addScheduleByEmployee(dataShedule)
                  );
            Assert.Equal("Doesnt exit workplace with that id", exception.Message);

        }
    }
}
