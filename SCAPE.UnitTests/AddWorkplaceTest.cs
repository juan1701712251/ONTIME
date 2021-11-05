
using SCAPE.Application.Interfaces;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;

using System.Threading.Tasks;
using Xunit;

namespace SCAPE.UnitTests
{
    public class AddWorkplaceTest

    {
        private readonly IWorkPlaceService _workplaceService;


        public AddWorkplaceTest(IWorkPlaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }
        [Fact]
        public async Task AddWorkplaceValid()
        {
            //workplace's data and  employeer's data is ok
            WorkPlace workplace = new WorkPlace();
            workplace.Name = "Panadería Julio";
            workplace.Address = "Cll 23 # 12 - 10";
            workplace.Description = "Delicioso";
            workplace.LatitudePosition = "4.0";
            workplace.LongitudePosition = "-72.0";


            int workplaceId = await _workplaceService.insertWorkPlace(workplace, "employeer@ontime.com","prueba");
            Assert.NotNull(workplaceId);


        }
        [Fact]
        public async Task AddWorkplaceWithEmailInvalid()
        {
            //Email´s Employeer in invalid
            WorkPlace workplace = new WorkPlace();
            workplace.Name = "Panadería Julio";
            workplace.Address = "Cll 23 # 12 - 10";
            workplace.Description = "Delicioso";
            workplace.LatitudePosition = "4.0";
            workplace.LongitudePosition = "-72.0";


          
            EmployerException exception = await Assert.ThrowsAsync<EmployerException>(
                     () => _workplaceService.insertWorkPlace(workplace, "employ@ontime.com", "prueba")
                  );

            Assert.Equal("There is no Employer with that email", exception.Message);


        }
        [Fact]
        public async Task AddWorkplaceWithEmptyAddress()
        {
            //Workplace´s Address is Empty
            WorkPlace workplace = new WorkPlace();
            workplace.Description = "Delicioso";
            workplace.LatitudePosition = "4.0";
            workplace.LongitudePosition = "-72.0";


            WorkPlaceException exception = await Assert.ThrowsAsync<WorkPlaceException>(
                     () => _workplaceService.insertWorkPlace(workplace, "employeer@ontime.com", "prueba")
                  );

            Assert.Equal("There was an error insert WorkPlace. Please verify fields", exception.Message);




        }
        [Fact]
        public async Task AddWorkplaceWithEmptyName()
        {
            //Workplace´s Name is Empty
            WorkPlace workplace = new WorkPlace();
            workplace.Address = "Cll 23 # 12 - 10";
            workplace.Description = "Delicioso";
            workplace.LatitudePosition = "4.0";
            workplace.LongitudePosition = "-72.0";


            WorkPlaceException exception = await Assert.ThrowsAsync<WorkPlaceException>(
                     () => _workplaceService.insertWorkPlace(workplace, "employeer@ontime.com", "prueba")
                  );

            Assert.Equal("There was an error insert WorkPlace. Please verify fields", exception.Message);
        }
        [Fact]
        public async Task AddWorkplaceWithEmptyCoord()
        {
            //Workplace´s coordenates is Empty
            WorkPlace workplace = new WorkPlace();
            workplace.Name = "Panadería Julio";
            workplace.Address = "Cll 23 # 12 - 10";
            workplace.Description = "Delicioso";

            WorkPlaceException exception = await Assert.ThrowsAsync<WorkPlaceException>(
                     () => _workplaceService.insertWorkPlace(workplace, "employeer@ontime.com", "prueba")
                  );

            Assert.Equal("There was an error insert WorkPlace. Please verify fields", exception.Message);


        }




    }
}