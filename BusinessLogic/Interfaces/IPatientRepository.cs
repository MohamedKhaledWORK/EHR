using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        public Task<Patient> GetByPatientNameAsync(string Username);
    }
}
