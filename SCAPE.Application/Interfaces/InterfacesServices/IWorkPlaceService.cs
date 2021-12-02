using SCAPE.Application.DTOs;
using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCAPE.Application.Interfaces
{
    public interface IWorkPlaceService
    {
        Task<int> insertWorkPlace(WorkPlace workPlace,string emailEmployer, string faceListID);
        Task<List<WorkPlace>> getAll(string emailEmployer);
        Task<WorkPlace> getEmployeesWithImageByWorkplace(int workPlaceId);
        Task<bool> editWorkPlace(WorkPlace editWorkPlace, int workPlaceId,string emailEmployer);
        Task<bool> deleteWorkPlace(int workplaceId, string emailEmployer);
        Task<List<WorkPlace>> getWorkPlaceNearLocation(string latitude, string longitude, double precision);
    }
}
