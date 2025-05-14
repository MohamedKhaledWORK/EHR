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
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly EHRdbContext _dbContext;

        public PatientRepository(EHRdbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Patient> GetByPatientNameAsync(string username) =>    await _dbContext.Patients.Where(S => S.Username == username).FirstOrDefaultAsync();

    }
}
