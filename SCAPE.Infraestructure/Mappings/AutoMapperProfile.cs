using AutoMapper;
using SCAPE.Application.DTOs;
using SCAPE.Domain.Entities;

namespace SCAPE.Infraestructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeCreateDTO>();
            CreateMap<EmployeeCreateDTO, Employee>();
            CreateMap<EmployeeWorkPlaceDTO, EmployeeWorkPlace>();
            CreateMap<EmployeeWorkPlace, EmployeeWorkPlaceDTO>(); 
            CreateMap<Employee, EmployeeWithImageDTO>();
            CreateMap<EmployeeImage, EmployeeImageDTO>(); 
        }
       
    }
}
