using Microsoft.Extensions.Configuration;
using Moq;
using SCAPE.Application.DTOs;
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
    public class AuthUserTest

    {
        private readonly IUserService _userService;


        public AuthUserTest(IUserService userService)
        {
            _userService = userService;
        }

        [Fact]
        public async Task LoginWithOKData()
        {
            //Employee´s data is username:jefecarlos@gmail.com and password:colanta2409
            User user = await _userService.login("jefecarlos@gmail.com", "colanta2409");
            Assert.NotNull(user);
            Assert.Equal(user.Role,"Employee");
        }
        [Fact]
        public async Task noLoginWithDocumentId()
        {
            //Employee´s fails data is username:1098736511 and password:colanta2409
            UserException exception = await Assert.ThrowsAsync<UserException>(
                     () => _userService.login("1098736511", "colanta2409")
                  );
            Assert.Equal("There was an error with credentials", exception.Message);
        }
        [Fact]
        public async Task noLoginWithEmptyData()
        {
            //Employee´s data in this test is empty.
            UserException exception = await Assert.ThrowsAsync<UserException>(
                     () => _userService.login("jefecarlos@gmail.com", "")
                  );
            Assert.Equal("There was an error with credentials", exception.Message);
        }
        [Fact]
        public async Task noLoginWithIncorrectPassword()
        {
            //Employee´s password in this test is abcd123.
            UserException exception = await Assert.ThrowsAsync<UserException>(
                     () => _userService.login("jefecarlos@gmail.com", "abcd123 ")
                  );
            Assert.Equal("There was an error with credentials", exception.Message);
        }


    }
}