using Microsoft.Extensions.Configuration;
using Moq;
using SCAPE.Application.Services;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.FaceRecognition;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SCAPE.UnitTests
{
    public class ListEmployeesTest

    {
        private readonly IConfiguration _configuration;


      
        public ListEmployeesTest()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }

        [Fact]
        public async Task ListEmployeesByEmptyWorkPlace()
        {

        }

        [Fact]
        public async Task ListEmployeesByWorkPlace()
        {

        }

    }
}