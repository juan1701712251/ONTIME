using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCAPE.Application.Interfaces;
using SCAPE.Application.Services;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.Context;
using SCAPE.Infraestructure.FaceRecognition;
using SCAPE.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCAPE.UnitTests
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            //Dependencias
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IEmployee_WorkPlaceRepository, Employee_WorkPlaceRepository>();
            services.AddTransient<IEmployeeService, EmployeeService>();


            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IAttendanceRepository, AttendanceRepository>();
            services.AddTransient<IAttendanceService, AttendanceService>();

            services.AddTransient<IFaceRecognition, FaceRecognition>();

            services.AddTransient<ITokenService, TokenService>();
            //DBContext
            services.AddDbContext<SCAPEDBContext>(options => options.UseSqlServer("Server=tcp:scape.database.windows.net,1433;Initial Catalog=SCAPEDB;Persist Security Info=False;User ID=scape_admin;Password=Encoding0605;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30"));
        }
    }
}
