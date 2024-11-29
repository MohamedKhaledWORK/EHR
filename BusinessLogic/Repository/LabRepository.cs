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
    public class LabRepository : GenericRepository<Lab>, ILabRepository
    {
        private readonly EHRdbContext _dbContext;

    public LabRepository(EHRdbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

        public async Task<IEnumerable<Lab>> GetLabByPatientNameAsync(string Username)
        {
            return await _dbContext.LabTests.Where(S => S.Patient.Username == Username).Include(P=>P.Patient).ToListAsync();
        }
    }
}
