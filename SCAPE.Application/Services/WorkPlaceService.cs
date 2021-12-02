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
        private readonly IEmployee_WorkPlaceRepository _employee_WorkPlaceRepository;

        public WorkPlaceService(IWorkPlaceRepository workPlaceRepository,IEmployerRepository employerRepository,IEmployee_WorkPlaceRepository employee_WorkPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
            _employerRepository = employerRepository;
            _employee_WorkPlaceRepository = employee_WorkPlaceRepository;
        }

        /// <summary>
        /// Get All Employees With Image By WorkPlace ID
        /// </summary>
        /// <returns></returns>
        public async Task<WorkPlace> getEmployeesWithImageByWorkplace(int workPlaceId)
        {
            WorkPlace workplace = await _employee_WorkPlaceRepository.getEmployeesWithImageByWorkPlace(workPlaceId);
            if (workplace == null)
            {
                throw new EmployeeWorkPlaceException("There is no Workplace with that ID");
            }
            return workplace;
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

        public async Task<WorkPlace> findWorkPlaceById(int workPlaceId)
        {
            return await _workPlaceRepository.get(workPlaceId);
        }

        /// <summary>
        /// Edit a Workplace Service
        /// </summary>
        /// <param name="editWorkPlace">Object with data to Edit</param>
        /// <returns>If calls is succesful returns true</returns>
        public async Task<bool> editWorkPlace(WorkPlace editWorkPlace, int workPlaceId,string emailEmployer)
        {
            //Get WorkPlace

            WorkPlace ctWorkPlace = await findWorkPlaceById(workPlaceId);

            if (ctWorkPlace == null)
            {
                throw new WorkPlaceException("There is no WorkPlace with that Id");
            }

            //Verify that WorkPlace belong to Employer 

            Employer e = await _employerRepository.findEmployerByEmail(emailEmployer);

            if(ctWorkPlace.IdEmployer != e.Id)
            {
                throw new WorkPlaceException("This employer can't edit this Workplace");
            }
            
            bool isEdit = await _workPlaceRepository.editWorkPlace(editWorkPlace,ctWorkPlace);

            if (!isEdit)
            {
                throw new WorkPlaceException("There was an error editing WorkPlace. Please verify fields");
            }

            return isEdit;
        }

        /// <summary>
        /// Delete a WorkPlace
        /// </summary>
        /// <param name="workplaceId">Workplace's Id</param>
        /// <param name="emailEmployer">Email to verify</param>
        /// <returns>If calls is succesful returns true</returns>
        public async Task<bool> deleteWorkPlace(int workplaceId, string emailEmployer)
        {
            //Get WorkPlace

            WorkPlace ctWorkPlace = await findWorkPlaceById(workplaceId);

            if (ctWorkPlace == null)
            {
                throw new WorkPlaceException("There is no WorkPlace with that Id");
            }

            //Verify that WorkPlace belong to Employer 

            Employer e = await _employerRepository.findEmployerByEmail(emailEmployer);

            if (ctWorkPlace.IdEmployer != e.Id)
            {
                throw new WorkPlaceException("This employer can't delete this Workplace");
            }

            bool isDelete = await _workPlaceRepository.deleteWorkPlace(ctWorkPlace);

            if (!isDelete)
            {
                throw new WorkPlaceException("There was an error deleting WorkPlace");
            }

            return isDelete;
        }

        public async Task<List<WorkPlace>> getWorkPlaceNearLocation(string latitude, string longitude, double precision)
        {
            //TODO: Limpiar y verificar datos de longitude y latitude

            double latitudeOrigin = Double.Parse(latitude.Replace(".",","));
            double longitudeOrigin = Double.Parse(longitude.Replace(".", ","));

            List<WorkPlace> workplaces = await _workPlaceRepository.getAllWorkPlaces();
            List<WorkPlace> outWorkPlaces = new List<WorkPlace>();
            foreach (WorkPlace w in workplaces)
            {
                if (w.LongitudePosition == null || w.LatitudePosition == null)
                {
                    continue;
                }
                double latitudeWorkPlace = Double.Parse(w.LatitudePosition.Replace(".", ","));
                double longitudeWorkPlace = Double.Parse(w.LongitudePosition.Replace(".", ","));
                //TODO: Limpiar y verificar datos de longitude y latitude

                if (DistanceTo(latitudeOrigin, longitudeOrigin, latitudeWorkPlace, longitudeWorkPlace,'m') <= precision)
                {
                    outWorkPlaces.Add(w);
                }
            }

            return outWorkPlaces;

        }

        public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'm': //Meters
                    return (dist * 1.609344) * 1000;
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}
