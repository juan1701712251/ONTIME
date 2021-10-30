using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SCAPE.Domain.Entities;
using SCAPE.Domain.Interfaces;
using SCAPE.Infraestructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCAPE.Infraestructure.Repositories
{
    public class WorkPlaceRepository : IWorkPlaceRepository
    {
        private readonly SCAPEDBContext _context;

        public WorkPlaceRepository(SCAPEDBContext context)
        {
            _context = context;
        }

        public async Task<bool> deleteWorkPlace(WorkPlace ctWorkPlace)
        {
            try
            {
                _context.WorkPlace.Remove(ctWorkPlace);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> editWorkPlace(WorkPlace editWorkPlace,WorkPlace ctWorkPlace)
        {
            try
            {
                ctWorkPlace.Name = editWorkPlace.Name ?? ctWorkPlace.Name;
                ctWorkPlace.Address = editWorkPlace.Address ?? ctWorkPlace.Address;
                ctWorkPlace.LatitudePosition = editWorkPlace.LatitudePosition ?? ctWorkPlace.LatitudePosition;
                ctWorkPlace.LongitudePosition = editWorkPlace.LongitudePosition ?? ctWorkPlace.LongitudePosition;
                ctWorkPlace.Description = editWorkPlace.Description ?? ctWorkPlace.Description;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<WorkPlace> get(int workPlaceId)
        {
            return await _context.WorkPlace.FirstOrDefaultAsync(w => w.Id == workPlaceId);
        }

        /// <summary>
        /// Get al Workplace
        /// </summary>
        /// <param name="idEmployer">Employer's Id</param>
        /// <returns>List of workplaces corresponding Employer's id</returns>
        public async Task<List<WorkPlace>> getAll(int idEmployer)
        {
            List<WorkPlace> workPlaces = await _context.WorkPlace.Where(w => w.IdEmployer == idEmployer).ToListAsync(); 
            return workPlaces;
        }
        /// <summary>
        /// Insert a new WorkPlace
        /// </summary>
        /// <param name="workPlace">Objecto Workplace with data</param>
        /// <returns>If insert is succesful return id</returns>
        public async Task<int> insertWorkPlace(WorkPlace workPlace)
        {
            try
            {
                _context.WorkPlace.Add(workPlace);
                await _context.SaveChangesAsync();
                return workPlace.Id;
            }
            catch (Exception ex) 
            {
                return -1;
            }
        }
    }
}


