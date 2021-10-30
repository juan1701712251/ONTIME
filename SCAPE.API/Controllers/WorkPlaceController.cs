using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCAPE.API.ActionsModels;
using SCAPE.Application.DTOs;
using SCAPE.Application.Interfaces;
using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCAPE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPlaceController : ControllerBase
    {
        private readonly IWorkPlaceService _workPlaceService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public WorkPlaceController(IWorkPlaceService workPlaceService,IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _workPlaceService = workPlaceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new WorkPlace
        /// </summary>
        /// <param name="workPlaceDTO">Data of workplace</param>
        /// <returns>If insert is succesful return id workplace</returns>
        /// <response code = "400">EmployerException --> There is no Employer with that email<br></br>
        ///                         WorkPlaceException --> There was an error insert WorkPlace. Please verify fields</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> addWorkPlace(WorkPlaceDTO workPlaceDTO)
        {
            //Get Email of JWT
            
            WorkPlace newWorkPlace = _mapper.Map<WorkPlace>(workPlaceDTO);
            newWorkPlace.LatitudePosition = workPlaceDTO.Latitude;
            newWorkPlace.LongitudePosition = workPlaceDTO.Longitude;

            var claimsIdentity = User.Identity as ClaimsIdentity;
            string emailEmployer = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            
            int idWorkPlace;

            try
            {
                idWorkPlace = await _workPlaceService.insertWorkPlace(newWorkPlace,emailEmployer, _configuration.GetValue<string>("AzureAPI:FaceListID"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(idWorkPlace);
        }

        /// <summary>
        /// Edit a Workplace
        /// </summary>
        /// <param name="workPlaceDTO">Object with workplace's data</param>
        /// <param name="workplaceId">Workplace's Id</param>
        /// <returns>If edit is succesful return true</returns>
        /// <response code = "400">WorkPlaceException --> There is no WorkPlace with that Id<br></br>
        ///                         WorkPlaceException --> This employer can't edit this Workplace<br></br>
        ///                         WorkPlaceException --> There was an error editing WorkPlace. Please verify fields</response>
        [HttpPut]
        [Authorize(Roles = "Admin,Employer")]
        [Route("{workplaceId}")]
        public async Task<IActionResult> updateWorkPlace(WorkPlaceUpdateDTO workPlaceDTO, int workplaceId)
        {
            WorkPlace editWorkPlace = _mapper.Map<WorkPlace>(workPlaceDTO);
            editWorkPlace.LatitudePosition = workPlaceDTO.Latitude;
            editWorkPlace.LongitudePosition = workPlaceDTO.Longitude;

            var claimsIdentity = User.Identity as ClaimsIdentity;
            string emailEmployer = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            bool isEdit;

            try
            {
                isEdit = await _workPlaceService.editWorkPlace(editWorkPlace,workplaceId,emailEmployer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(isEdit);
        }

        /// <summary>
        /// Delete a WorkPlace By Id
        /// </summary>
        /// <param name="workplaceId">WorkPlace's Id</param>
        /// <returns>If delete is succesful return true</returns>
        /// <response code = "400">WorkPlaceException -->There is no WorkPlace with that Id<br></br>
        ///                         WorkPlaceException --> This employer can't delete this Workplace<br></br>
        ///                         WorkPlaceException --> There was an error deleting WorkPlace. Please verify fields</response>
        [HttpDelete]
        [Authorize(Roles = "Admin,Employer")]
        [Route("{workplaceId}")]
        public async Task<IActionResult> deleteWorkPlace(int workplaceId)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string emailEmployer = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            bool isDelete;

            try
            {
                isDelete = await _workPlaceService.deleteWorkPlace(workplaceId, emailEmployer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(isDelete);
        }


        /// <summary>
        /// Get All WorkPlace of User (Employer)
        /// </summary>
        /// <returns>If insert is succesful return List of workplace</returns>
        /// <response code = "400">EmployerException --> There is no Employer with that email<br></br>
        ///                         WorkPlaceException --> There is no workplaces for this employer</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> getAllWorkPlace()
        {
            //Get Email of JWT
            
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string emailEmployer = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            List<WorkPlace> workPlaces;

            try
            {
                workPlaces = await _workPlaceService.getAll(emailEmployer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            List<WorkPlaceDATAOUTDTO> workPlacesDTO = _mapper.Map<List<WorkPlaceDATAOUTDTO>>(workPlaces);

            return Ok(workPlacesDTO);
        }

        /// <summary>
        /// Get All Employees With Image By WorkPlace
        /// </summary>
        /// <param name="workplaceId">WorkPlace Id</param>
        /// <returns>WorkPlace with List of employees </returns>
        /// <response code = "400">EmployeeWorkPlaceException --> There is no Workplace with that ID</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Employer")]
        [Route("{workplaceId}")]
        public async Task<IActionResult> getEmployeesWithImage(int workplaceId)
        {
            WorkPlace workplace = null;
            try
            {
                workplace = await _workPlaceService.getEmployeesWithImageByWorkplace(workplaceId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            WorkPlaceWithEmployeesDTO workPlaceWithEmployeesDTO = _mapper.Map<WorkPlaceWithEmployeesDTO>(workplace);

            return Ok(workPlaceWithEmployeesDTO);
        }
    }
}
