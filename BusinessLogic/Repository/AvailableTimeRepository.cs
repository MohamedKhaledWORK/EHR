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
    public class AvailableTimeRepository : GenericRepository<DoctorAvailability> , IAvailableTimeRepositry
    {
        private readonly EHRdbContext _dbContext;

        public AvailableTimeRepository(EHRdbContext dbContext): base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DoctorAvailability>> GetAllById(int Id)
        {
            return await _dbContext.DoctorAvailabilities.Where(DA => DA.DoctorId == Id).Include(DA => DA.Doctor).Include(DA=>DA.Doctor.Staff).ToListAsync();
        }
    }
}
