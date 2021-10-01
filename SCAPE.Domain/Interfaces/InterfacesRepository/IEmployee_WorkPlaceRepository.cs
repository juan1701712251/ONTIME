using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IEmployee_WorkPlaceRepository
    {
        Task<List<EmployeeWorkPlace>> getEmployeesWithImageByWorkPlace(int workPlaceId);
    }
}
