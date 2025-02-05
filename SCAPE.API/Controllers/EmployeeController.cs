﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCAPE.API.ActionsModels;
using SCAPE.Application.DTOs;
using SCAPE.Application.Interfaces;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;


        public EmployeeController(IEmployeeService employeeService,IMapper mapper,IUserService userService)
        {
            _employeeService = employeeService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Insert a Employee from web service
        /// </summary>
        /// <param name="employeeDTO">Object in DTO (Data Transfer Object) Format</param>
        /// <returns>If insert is succesful, return a "Code status 200"</returns>
        /// <remarks>
        ///         <para>Exceptions</para>
        ///         <para>RegisterEmployeeException --> The document, name and lastname fields are required</para>
        ///         <para>RegisterEmployeeException --> Email address entered is not valid</para>
        ///         <para>RegisterEmployeeException --> Document entered is not valid</para>
        ///         <para>RegisterEmployeeException --> Sex entered is not valid</para>
        ///         <para>RegisterEmployeeException --> An employee with the same document id has already been registered</para>
        ///         <para>RegisterEmployeeException --> An employee with the same email has already been registered</para>
        /// </remarks>
        /// <response code = "400">
        /// RegisterEmployeeException --> The document, name and lastname fields are required<br></br>
        /// RegisterEmployeeException --> Email address entered is not valid<br></br>
        /// RegisterEmployeeException --> Document entered is not valid<br></br>
        /// RegisterEmployeeException --> Sex entered is not valid<br></br>
        /// RegisterEmployeeException --> An employee with the same document id has already been registered<br></br>
        /// RegisterEmployeeException --> An employee with the same email has already been registered
        /// </response>
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> insertEmployee(EmployeeCreateDTO employeeDTO)
        {   
            Employee employee = _mapper.Map<Employee>(employeeDTO);

            try
            {
                await _employeeService.insertEmployee(employee);
                await _userService.addUser(employee.Email, employeeDTO.Password, "Employee");
                //It create association with workplace, the date is current by default
                if (employeeDTO.WorkPlaceId != 0)
                {
                    await _employeeService.addWorkPlaceByEmployee(employeeDTO.DocumentId, employeeDTO.WorkPlaceId, DateTime.Now, DateTime.Now);
                }

            }
            catch(UserException ex)
            {
                string email = await _employeeService.deleteEmployee(employee.DocumentId);
                if (email != null)
                    await _userService.deleteUser(email);
                return BadRequest(ex.Message);
            }catch(EmployeeWorkPlaceException ex)
            {
                string email = await _employeeService.deleteEmployee(employee.DocumentId);
                if (email != null)
                    await _userService.deleteUser(email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
            return Ok("Employee has been created");
        }

        /// <summary>
        /// Delete a employee from web service
        /// </summary>
        /// <param name="documentId">Document´s Employee</param>
        /// <returns>If delete is succesful, return a "Code status 200"</returns>
        /// <remarks>
        ///         <para>Exceptions</para>
        ///         <para>EmployeeException --> Employee with that document doesnt exist</para>
        ///         <para>EmployeeException --> There was an error deleting the employee </para>
        ///         <para>EmployeeException --> There was an error deleting the Face from Azure. Anyway, the employee was deleted</para>
        /// </remarks>
        /// <response code = "400">
        /// EmployeeException --> Employee with that document doesnt exist<br></br>
        /// EmployeeException --> There was an error deleting the employee <br></br>
        /// EmployeeException --> There was an error deleting the Face from Azure. Anyway, the employee was deleted
        /// </response>
        [HttpDelete]
        [Authorize(Roles = "Admin, Employer")]
        [Route("{documentId}")]
        public async Task<IActionResult> deleteEmployee(string documentId)
        {
            try
            {
                string email = await _employeeService.deleteEmployee(documentId);
                if(email != null)
                    await _userService.deleteUser(email);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Employee has been deleted");
        }

        /// <summary>
        /// Edit a Employee
        /// If password equal "" or null, the old password will be left
        /// </summary>
        /// <param name="documentIdOLD">Documento antiguo del Empleado</param>
        /// <param name="employeeDTO">Object in DTO (Data Transfer Object) Format</param>
        /// <returns>If edit is succesful, return a "Code status 200"</returns>
        /// <remarks>
        ///         <para>Exceptions</para>
        ///         <para>EmployeeException --> There was an error editing the employee </para>
        /// </remarks>
        /// <response code = "400">
        /// EmployeeException --> There was an error editing the employee 
        /// </response>
        [HttpPut]
        [Authorize(Roles = "Admin,Employer")]
        [Route("{documentIdOLD}")]
        public async Task<IActionResult> editEmployee(string documentIdOLD,EmployeeEditDTO employeeDTO)
        {
            Employee employeeEdit = _mapper.Map<Employee>(employeeDTO);
            bool resultAssociate = false;
            try
            {
                resultAssociate = await _employeeService.editEmployee(documentIdOLD,employeeEdit);

                if(employeeDTO.Email != null && employeeDTO.Password != null && employeeDTO.Password != "" && resultAssociate)
                    await _userService.editUser(employeeDTO.Email,employeeDTO.Password,"Employee");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(resultAssociate);
        }

        /// <summary>
        /// Associate a face to an Employee
        /// </summary>
        /// <param name="data">Model with documentId, EncodeImage and faceListId in AsoociateFaceModel class </param>
        /// <returns>If  associate is succesfull, return a "Code status 200" and bool true </returns>
        /// <remarks>
        ///         <para>Exceptions</para>
        ///         <para>FaceRecognitionException --> The image has already been associated with an employee </para>
        ///         <para>EmployeeDocumentException --> Employee's document is not valid</para>
        /// </remarks>
        /// <response code = "400">
        /// FaceRecognitionException --> The image has already been associated with an employee <br></br>
        /// EmployeeDocumentException --> Employee's document is not valid
        /// </response>
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        [Route("AssociateImage")]
        public async Task<IActionResult> associateFace(AssociateFaceModel data)
        {
           
            string documentId = data.documentId;
            string encodeImage = data.encodeImage;
            string faceListId = data.faceListId;

            bool resultAssociate = false;

            try {
                resultAssociate = await _employeeService.associateFace(documentId, encodeImage, faceListId);
            }catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            return Ok(resultAssociate);

        }
        /// <summary>
        /// Get Employee by Face Recognition
        /// </summary>
        /// <param name="data">Model with faceListId and EncodeImage in FindEmployeeModel class</param>
        /// <returns>If get is succesfull, return a Employee and "Code status 200"</returns>       
        /// <remarks>
        ///         <para>Exceptions</para>
        ///         <para>FaceRecognitionException --> No persistedFaceid found for this face</para>
        ///         <para>EmployeeException --> No Employee found in Database for this persistedFaceId</para>
        /// </remarks>
        /// <response code = "400">
        /// FaceRecognitionException --> No persistedFaceid found for this face <br></br>
        /// EmployeeException --> No Employee found in Database for this persistedFaceId
        /// </response>
        [HttpPost]
        [Authorize]
        [Route("GetEmployeeByImage")]
        public async Task<IActionResult> getEmployeeByFace(FindEmployeeModel data)
        {
            string encodeImage = data.encodeImage;
            string faceListId = data.faceListId;

            EmployeeDTO employeeDTO = new EmployeeDTO();

            try{
                Employee employee = await _employeeService.getEmployeeByFace(encodeImage, faceListId);
                employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            }catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            return Ok(employeeDTO);
        }
        /// <summary>
        /// Get Employee By Document ID
        /// </summary>
        /// <param name="documentId">Document Id</param>
        /// <returns>If get is succesfull, return a Employee and "Code status 200"</returns>
        /// <remarks>
        ///         <para>Exceptions</para>
        ///         <para>EmployeeException --> Employee doesnt exist with that document</para>
        /// </remarks>
        /// <response code = "400">EmployeeException --> Employee doesnt exist with that document</response>
        [HttpGet]
        [Authorize]
        [Route("{documentId}")]
        public async Task<IActionResult> getEmployeeByDocument(string documentId)
        {
            EmployeeWithImageDTO employeeDTO = new EmployeeWithImageDTO();

            try
            {
                Employee employee = await _employeeService.findEmployee(documentId);
                if (employee == null)
                {
                    throw new EmployeeException("Employee doesnt exist with that document");
                }
                employeeDTO = _mapper.Map<EmployeeWithImageDTO>(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(employeeDTO);
        }

        /// <summary>
        /// Add Info Schedule to Employee By WorkPlace
        /// </summary>
        /// <param name="dataScheduleModel">Data of Schedule</param>
        /// <returns>If call is successful return true</returns>
        /// <response code = "400">EmployeeException --> Doesnt exit employee with that document<br></br>
        ///                        EmployeeWorkPlaceException --> Doesnt exit workplace with that id<br></br> 
        ///                        EmployeeWorkPlaceException --> There was an error adding/updating the employee to workplace<br></br> 
        ///                        ScheduleException --> Please insert valid times for minutes<br></br> 
        ///                        ScheduleException --> Schedule has crossings<br></br> 
        ///                        ScheduleException --> There was an error checking schedules<br></br> 
        ///                        ScheduleException --> There was an error inserting new schedules
        /// </response>
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        [Route("schedule")]
        public async Task<IActionResult> addScheduleByEmployee(DataScheduleModelDTO dataScheduleModel)
        {
            bool result;
            try
            {
                result = await _employeeService.addScheduleByEmployee(dataScheduleModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a schedule from employee
        /// </summary>
        /// <param name="documentId">Employee's document</param>
        /// <param name="workPlaceId">Workplace identifier</param>
        /// <returns>If call is successful return List of Schedules </returns>
        /// <response code = "400">EmployeeException --> Doesnt exit employee with that document<br></br></response>
        [HttpGet]
        [Authorize(Roles = "Admin,Employer")]
        [Route("schedule")]
        public async Task<IActionResult> getScheduleByEmployee(string documentId,int workPlaceId)
        {
            List<ScheduleModelDTO> resultDTO;
            try
            {
                List<EmployeeSchedule> schedules = await _employeeService.getScheduleByEmployee(documentId,workPlaceId);
                resultDTO = _mapper.Map<List<ScheduleModelDTO>>(schedules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(resultDTO);
        }

        /// <summary>
        /// Delete Employee by WorkPlace
        /// </summary>
        /// <param name="documentId">Employee's document</param>
        /// <param name="workPlaceId">WorkPlace Identifier</param>
        /// <returns>If call is successful return true </returns>
        [HttpDelete]
        [Authorize(Roles = "Admin,Employer")]
        [Route("workplace")]
        public async Task<IActionResult> deleteEmployeeByWorkPlace(string documentId, int workPlaceId)
        {
            bool result;
            try
            {
                result = await _employeeService.deleteEmployeeByWorkPlace(documentId, workPlaceId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }
    }
}
