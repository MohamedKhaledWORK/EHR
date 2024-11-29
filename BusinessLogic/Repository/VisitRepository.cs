using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class VisitRepository : GenericRepository<Visit>, IVisitRepository
    {
        private readonly EHRdbContext _dbContext;

        public VisitRepository(EHRdbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Visit>> GetAllToDoctor(int id)
        {
            return await _dbContext.Visits.Where(V => V.Doctor.Id == id).Include(V => V.Doctor.Staff).Include(V => V.Patient).ToListAsync();
        }

        public async Task<IEnumerable<Visit>> GetAlltoPatientName(string UserName)
        {
            return await _dbContext.Visits.Where(V => V.Patient.Username == UserName).Include(V => V.Doctor.Staff).Include(V => V.Patient).ToListAsync();
        }
        public async Task<IEnumerable<Visit>> GetAlltoPatient(int id)
        {
            return await _dbContext.Visits.Where(V => V.Patient.Id == id).Include(V => V.Doctor.Staff).Include(V => V.Patient).ToListAsync();
        }

    }
}
