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
    }
}
