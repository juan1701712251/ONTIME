using Microsoft.Extensions.Configuration;
using Moq;
using SCAPE.Application.Interfaces;
using SCAPE.Application.Services;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.FaceRecognition;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SCAPE.UnitTests
{
    public class ListWorkplaceTest

    {
        private readonly IWorkPlaceService _workplaceService;


        public ListWorkplaceTest(IWorkPlaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        [Fact]
        public async Task ListAllWorkplace()
        {
            //In Database workplace (2) is Empty
            List<WorkPlace> workplaces = await _workplaceService.getAll("employeer@ontime.com");
            Assert.NotNull(workplaces);
            Assert.NotEqual(workplaces.Count, 0);
        }

        [Fact]
        public async Task FailEmailEmployeer()
        {
            //Email employer@gmail.com does not exist

            EmployerException exception = await Assert.ThrowsAsync<EmployerException>(
                           () => _workplaceService.getAll("employer@gmail.com")
                        );

            Assert.Equal("There is no Employer with that email", exception.Message);
        }

        
    }
}