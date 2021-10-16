﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpPost]
        [Authorize(Roles = "Admin,Employeer")]
        public async Task<IActionResult> insertEmployee(EmployeeCreateDTO employeeDTO)
        {   
            Employee employee = _mapper.Map<Employee>(employeeDTO);

            try
            {
                await _employeeService.insertEmployee(employee);
                await _userService.addUser(employee.Email, employeeDTO.Password, "Employee");
                //It create association with wotkplace, the date is current by default
                if (employeeDTO.WorkPlaceId != 0)
                {
                    await _employeeService.addWorkPlaceByEmployee(employeeDTO.DocumentId, employeeDTO.WorkPlaceId, DateTime.Now, DateTime.Now, null);
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
        [HttpDelete]
        [Authorize(Roles = "Admin, Employeer")]
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

        [HttpPut]
        [Authorize(Roles = "Admin,Employeer")]
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
        [HttpPost]
        [Authorize(Roles = "Admin,Employeer")]
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
        /// Get All Employees With Image By WorkPlace
        /// </summary>
        /// <param name="workplaceId">WorkPlace Id</param>
        /// <returns>List Employees with data of WorkPlace</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Employeer")]
        [Route("GetEmployeesByWorkPlace/{workplaceId}")]
        public async Task<IActionResult> getEmployeesWithImage(int workplaceId)
        {
            List<EmployeeWorkPlace> employees = null;
            try
            {
                employees = await _employeeService.getEmployeesWithImageByWorkplace(workplaceId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            List<EmployeeWorkPlaceDTO> employeesDTO = _mapper.Map<List<EmployeeWorkPlaceDTO>>(employees);

            return Ok(employeesDTO);
        }
    }
}
