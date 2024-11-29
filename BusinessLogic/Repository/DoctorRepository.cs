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
    public class DoctorRepository : GenericRepository<Doctors>, IDoctorRepository
    {
        private readonly EHRdbContext _dbContext;

        public DoctorRepository(EHRdbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Doctors> GetDoctorByStaffID(int Id)
        {
            return await _dbContext.Doctors.Where(D => D.Staff.Id == Id).Include(D => D.Staff).Include(d=>d.Availabilities).FirstOrDefaultAsync();
        }
    }
}
