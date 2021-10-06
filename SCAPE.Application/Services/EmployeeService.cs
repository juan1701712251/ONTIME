﻿using SCAPE.Application.DTOs;
using SCAPE.Application.Interfaces;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCAPE.Application.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFaceRecognition _faceRecognition;
        private readonly IEmployee_WorkPlaceRepository _employee_WorkPlaceRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IFaceRecognition faceRecognition,IEmployee_WorkPlaceRepository employee_WorkPlaceRepository)
        {
            _employeeRepository = employeeRepository;
            _faceRecognition = faceRecognition;
            _employee_WorkPlaceRepository = employee_WorkPlaceRepository; 
        }
        /// <summary>
        /// This method contain bussiness logic 
        /// Associate face to an Employee, and save the employee's image in the database
        /// </summary>
        /// <param name="documentId">string with employee document id</param>
        /// <param name="encodeImage">string with encoded image</param>
        /// <param name="faceListId">string with the id of the face list to be associated with</param>
        /// <returns>
        /// if there is not employee return a error message,
        /// if there is not face detected return a error message,
        /// if there is already register face return a error message,
        /// if insert success, return true
        /// </returns>
        public async Task<bool> associateFace(string documentId, string encodeImage, string faceListId)
        {
            Employee employee = await findEmployee(documentId);

            if(employee == null)
            {
                throw new EmployeeDocumentException("Employee's document is not valid");
            }

            Face faceDetected = await identifyFaceInImage(encodeImage);

            string alreadyAssociate = await _faceRecognition.findSimilar(faceDetected, faceListId);

            if (alreadyAssociate != null)
            {
                throw new FaceRecognitionException("The image has already been associated with an employee");
            }

            string persistenFaceId = await _faceRecognition.addFaceAsync(encodeImage, faceListId);

            byte[] bytesImage = Convert.FromBase64String(encodeImage);
            await _employeeRepository.saveImageEmployee(new EmployeeImage(persistenFaceId, employee.Id, bytesImage));

            return true;
        }
        /// <summary>
        /// This method contain bussiness logic 
        /// Get employee by face image, obtains the persitence face id of an image and 
        /// verifies if it is registered in the database
        /// </summary>
        /// <param name="encodeImage">string with encoded image</param>
        /// <param name="faceListId">string with face list id</param>
        /// <returns>
        /// if there is not face detected return a error message,
        /// if there is not face similiar return a error message,
        /// if there is not associate employee at face, return a error message,
        /// if get success, return Employee
        /// </returns>
        public async Task<Employee> getEmployeeByFace(string encodeImage, string faceListId)
        {
            Face faceDetected = await identifyFaceInImage(encodeImage);

            string persistedFaceId = await _faceRecognition.findSimilar(faceDetected, faceListId);

            if (persistedFaceId == null)
            {
                throw new FaceRecognitionException("No persistedFaceid found for this face");
            }

            Employee employee = await _employeeRepository.findEmployeeByPersistedFaceId(persistedFaceId);

            if (employee == null)
            {
                throw new EmployeeException("No Employee found in Database for this persistedFaceId");
            }

            return employee;
        }

        /// <summary>
        /// Identify the face in the input image 
        /// </summary>
        /// <param name="encodeImage">string with encoded image</param>
        /// <returns>If get success, return Face object of the identified face
        /// if there is not face detected or there is more than one face
        /// return a error message</returns>
        public async Task<Face> identifyFaceInImage(string encodeImage)
        {
            Face faceDetected = await _faceRecognition.detectFaceAsync(encodeImage);

            if (faceDetected == null)
            {
                throw new FaceRecognitionException("The image must contain only one face");
            }

            return faceDetected;
        }

        /// <summary>
        /// This method contain bussiness logic
        /// Insert employee to _employeeRepository
        /// </summary>
        /// <param name="employee">Employee to insert</param>
        /// <returns>
        /// return a error message, if the document name and lastname fields are empty
        /// return a error message, if the employee's Email is not valid,
        /// return a error message, if the employee's document is not valid,
        /// return a error message, if the employee's sex is not valid,
        /// return a error message, if the employee's document already exists,
        /// return a error message, if the employee's email already exists,
        /// if get success, return Employee
        /// </returns>
        public async Task insertEmployee(Employee employee)
        {

            if(String.IsNullOrWhiteSpace(employee.DocumentId) || String.IsNullOrWhiteSpace(employee.FirstName) || String.IsNullOrWhiteSpace(employee.LastName))
                throw new RegisterEmployeeException("The document, name and lastname fields are required");

            if (employee.Email != null && !isValidEmail(employee.Email))
                throw new RegisterEmployeeException("Email address entered is not valid");

            if (!isValidDocument(employee.DocumentId))
                throw new RegisterEmployeeException("Document entered is not valid");

            if (employee.Sex != null && (employee.Sex.Length >= 2 || String.IsNullOrWhiteSpace(employee.Sex)))
                throw new RegisterEmployeeException("Sex entered is not valid");

            Employee foundEmployee = await findEmployee(employee.DocumentId);

            if (foundEmployee != null)
                throw new RegisterEmployeeException("An employee with the same document id has already been registered");
                    
            bool save = await _employeeRepository.insertEmployee(employee);

            if (!save)
                throw new RegisterEmployeeException("An employee with the same email has already been registered");

        }
        /// <summary>
        /// determines whether the employee email entered is valid
        /// </summary>
        /// <param name="email">string email from employee to add</param>
        /// <returns>
        /// if employee's email is valid, return true
        /// if employee's email is not valid, return false
        /// </returns>
        private static bool isValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return !String.IsNullOrWhiteSpace(email) && addr.Address == email ;
            }catch {
                return false;
            }
        }

        /// <summary>
        /// determines whether the employee document entered is valid
        /// </summary>
        /// <param name="documentId">DocumentId from employee to add</param>
        /// <returns>
        /// if employee's document is valid, return true
        /// if employee's document is not valid, return false
        /// </returns>
        private static bool isValidDocument(string documentId)
        {
            Regex regex = new Regex("^[1-9][0-9]+$");
            if (!String.IsNullOrWhiteSpace(documentId) && regex.IsMatch(documentId))
                return true;
            return false;
        }

        /// <summary>
        /// Find employee from repository
        /// </summary>
        /// <param name="documentId">DocumentId from employee to find</param>
        /// <returns>Employee associate to this documentId</returns>
        public async Task<Employee> findEmployee(string documentId)
        {
            Employee employee = await _employeeRepository.findEmployee(documentId);
            return employee;
        }

        /// <summary>
        /// Get All Employees 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> getEmployees()
        {
            return await _employeeRepository.getEmployees();
        }

        /// <summary>
        /// Get All Employees With Image By WorkPlace ID
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeWorkPlace>> getEmployeesWithImageByWorkplace(int workPlaceId)
        {
            return await _employee_WorkPlaceRepository.getEmployeesWithImageByWorkPlace(workPlaceId);
        }

        public async Task<bool> editEmployee(string documentIdOLD, Employee employeeEdit)
        {
            bool result = await _employeeRepository.editEmployee(documentIdOLD, employeeEdit);
            if (!result)
            {
                throw new EmployeeException("There was an error editing the employee ");
            }
            return result;
        }

        public async Task<string> deleteEmployee(string documentId)
        {
            string emailDelete = await _employeeRepository.deleteEmployee(documentId);
            if (emailDelete == null)
            {
                throw new EmployeeException("There was an error deleting the employee ");
            }
            return emailDelete;
        }
    }
}
