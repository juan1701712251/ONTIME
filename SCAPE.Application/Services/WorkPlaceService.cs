using SCAPE.Application.DTOs;
using SCAPE.Application.Interfaces;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SCAPE.Application.Services
{
    public class WorkPlaceService : IWorkPlaceService
    {

        private readonly IWorkPlaceRepository _workPlaceRepository;
        private readonly IEmployerRepository _employerRepository;

        public WorkPlaceService(IWorkPlaceRepository workPlaceRepository,IEmployerRepository employerRepository)
        {
            _workPlaceRepository = workPlaceRepository;
            _employerRepository = employerRepository;
        }

        /// <summary>
        /// Get all workplace
        /// </summary>
        /// <param name="emailEmployer">Employer's Email</param>
        /// <returns>If exist employer with that email, it returns List of workplace</returns>
        public async Task<List<WorkPlace>> getAll(string emailEmployer)
        {
            // Verify that email's employer exist in database and get this employer
            Employer employer = await _employerRepository.findEmployerByEmail(emailEmployer);

            if (employer == null)
            {
                throw new EmployerException("There is no Employer with that email");
            }

            // Insert in Repository
            List<WorkPlace> workPlaces = await _workPlaceRepository.getAll(employer.Id);

            if (workPlaces.Count == 0)
            {
                throw new WorkPlaceException("There is no workplaces for this employer");
            }

            return workPlaces;

        }

        /// <summary>
        /// Insert a new Workplace
        /// </summary>
        /// <param name="workPlace">workplace Data of DTO</param>
        /// <param name="emailEmployer">employer's email</param>
        /// <param name="faceListID">Facelistid associated to this workplace</param>
        /// <returns>If calls is succesful returns workplace id</returns>
        public async Task<int> insertWorkPlace(WorkPlace workPlace,string emailEmployer, string faceListID)
        {
            // Verify that email's employer exist in database and get this employer
            Employer employer = await _employerRepository.findEmployerByEmail(emailEmployer);

            if(employer == null)
            {
                throw new EmployerException("There is no Employer with that email");
            }

            // Create WorkPlace with faceListId, employerID, and Data of workplace
            workPlace.IdEmployer = employer.Id;
            workPlace.FaceListId = faceListID;

            // Insert in Repository
            int workPlaceId = await _workPlaceRepository.insertWorkPlace(workPlace);

            if (workPlaceId == -1)
            {
                throw new WorkPlaceException("There was an error insert WorkPlace. Please verify fields");
            }

            return workPlaceId;
        }
    }
}
