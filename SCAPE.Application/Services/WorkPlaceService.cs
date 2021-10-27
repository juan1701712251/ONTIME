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
