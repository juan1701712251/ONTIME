using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IWorkPlaceRepository
    {
        Task<int> insertWorkPlace(WorkPlace workPlace);
        Task<List<WorkPlace>> getAll(int idEmployer);
    }
}
