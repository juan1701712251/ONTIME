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
    }
}
