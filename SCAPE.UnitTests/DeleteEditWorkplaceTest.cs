using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using SCAPE.Application.DTOs;
using SCAPE.Application.Interfaces;
using SCAPE.Application.Services;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Exceptions;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.FaceRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SCAPE.UnitTests
{
    public class DeleteEditWorkplaceTest

    {
        private readonly IWorkPlaceService _workplaceService;


        public DeleteEditWorkplaceTest(IWorkPlaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        [Fact]
        public async Task EditWorkplaceWithNewName()
        {
            //Workplace´s id is "20" and new name is Bancolombia
            WorkPlace wokplaceEdit = new WorkPlace();
            wokplaceEdit.Name = "Bancolombia";
            wokplaceEdit.Address = "Cll 23 # 12 - 10";
            wokplaceEdit.Description = "Delicioso";
            wokplaceEdit.LatitudePosition = "4.0";
            wokplaceEdit.LongitudePosition = "-72.0";


            Assert.True(await _workplaceService.editWorkPlace(wokplaceEdit, 20 , "employeer@ontime.com"));


        }

        [Fact]
        public async Task EditAllFieldsWorkplaces()
        {
            //Workplace´s id is "20" and new fields are Name: Bancolombia, Address: "Cll 22 # 10 - 11, Description: Barato and Latitud : 12  Longitud:2

            WorkPlace wokplaceEdit = new WorkPlace();
            wokplaceEdit.Name = "Panadería Julio";
            wokplaceEdit.Address = "Cll 22 # 10 - 11";
            wokplaceEdit.Description = "Barato";
            wokplaceEdit.LatitudePosition = "12.0";
            wokplaceEdit.LongitudePosition = "2.0";

            Assert.True(await _workplaceService.editWorkPlace(wokplaceEdit, 20, "employeer@ontime.com"));


        }

        [Fact]
        public async Task EditWorkplaceWithIncorrectId()
        {
            //Edit A Workplace with an incorrect Id 


            WorkPlace wokplaceEdit = new WorkPlace();
            wokplaceEdit.Name = "Panadería Julio";
            wokplaceEdit.Address = "Cll 22 # 10 - 11";
            wokplaceEdit.Description = "Barato";
            wokplaceEdit.LatitudePosition = "12.0";
            wokplaceEdit.LongitudePosition = "2.0";


            WorkPlaceException exception = await Assert.ThrowsAsync<WorkPlaceException>(
                     () => _workplaceService.editWorkPlace(wokplaceEdit, 50, "employeer@ontime.com")
                  );
            Assert.Equal("There is no WorkPlace with that Id", exception.Message);

        }
        public async Task EditWorkplaceByIncorrecEmployeer()
        {
            //Edit A Workplace by an incorrect employeer 


            WorkPlace wokplaceEdit = new WorkPlace();
            wokplaceEdit.Name = "Panadería Julio";
            wokplaceEdit.Address = "Cll 22 # 10 - 11";
            wokplaceEdit.Description = "Barato";
            wokplaceEdit.LatitudePosition = "12.0";
            wokplaceEdit.LongitudePosition = "2.0";


            WorkPlaceException exception = await Assert.ThrowsAsync<WorkPlaceException>(
                     () => _workplaceService.editWorkPlace(wokplaceEdit, 20, "emp@gmail.com")
                  );
            Assert.Equal("This employer can't delete this Workplace", exception.Message);

        }
        [Fact]
        public async Task EditWorkplaceWithEmptyFields()
        {
            //Edit An workplace with an empty fields
            WorkPlace wokplaceEdit = new WorkPlace();
            wokplaceEdit.Address = "Cll 22 # 10 - 11";
            wokplaceEdit.Description = "Barato";
            wokplaceEdit.LatitudePosition = "12.0";
            wokplaceEdit.LongitudePosition = "2.0";

            WorkPlaceException exception = await Assert.ThrowsAsync<WorkPlaceException>(
                    () => _workplaceService.editWorkPlace(wokplaceEdit, 20, "employeer@ontime.com")
                 );
            Assert.Equal("There was an error editing WorkPlace. Please verify fields", exception.Message);
        }
        [Fact]
        public async Task RecoverDataWorkplace()
        {
            //This test is to recover an employee 


            WorkPlace wokplaceEdit = new WorkPlace();
            wokplaceEdit.Name = "Panadería Julio";
            wokplaceEdit.Address = "Cll 23 # 12 - 10";
            wokplaceEdit.Description = "Delicioso";
            wokplaceEdit.LatitudePosition = "4.0";
            wokplaceEdit.LongitudePosition = "-72.0";


            Assert.True(await _workplaceService.editWorkPlace(wokplaceEdit, 20, "employeer@ontime.com"));

        }

        [Fact]
        public async Task DeleteWorkplaceById()
        {
            
            Assert.True(await _workplaceService.deleteWorkPlace(38, "employeer@ontime.com"));

        }



    }
}