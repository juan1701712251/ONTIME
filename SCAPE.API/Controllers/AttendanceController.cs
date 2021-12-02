using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCAPE.API.ActionsModels;
using SCAPE.Application.Interfaces;
using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;

        public AttendanceController(IAttendanceService attendanceService,IMapper mapper)
        {
            _attendanceService = attendanceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a Attendance from Web Service
        /// </summary>
        /// <param name="data">>Object in DTO (Data Transfer Object) Format with data of attendance</param>
        /// <returns>
        /// If insert is fail, return a "Code error",
        /// If insert is succesful, return a "Code status 200"
        /// </returns>
        /// <response code = "400">
        /// AttendanceException --> The type of Attendance is a character, not a string<br></br>
        /// AttendanceException --> There was an error entering attendance.<br></br>
        /// AttendanceException --> There is not employee linked to that document
        /// </response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addAttendance(AttendanceModel data)
        {
            string documentEmployee = data.documentEmployee;
            int workPlaceId = data.workPlaceId;
            string type = data.type;
            DateTime date = data.dateTime;

            bool resultInsert = true;
            try
            {
                resultInsert = await _attendanceService.addAttendance(date, type, documentEmployee, workPlaceId);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(resultInsert);
        }

        /// <summary>
        /// Get all Attendances By Employee
        /// </summary>
        /// <param name="documentEmployee">Employee's Document</param>
        /// <returns>List a Employees</returns>
        /// <response code = "400">
        /// AttendanceException --> There was an error looking attendaces <br></br>
        /// EmployeeException --> There is not employee linked to that document
        /// </response>
        [HttpGet]
        [Authorize]
        [Route("{documentEmployee}")]
        public async Task<IActionResult> getAttendancesByEmployee(string documentEmployee)
        {

            List<Attendance> attendances;
            try
            {
                attendances = await _attendanceService.getAttendancesByEmployee(documentEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(attendances);
        }
    }
}
