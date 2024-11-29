using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IVisitRepository : IGenericRepository<Visit>
    {
        public Task<IEnumerable<Visit>> GetAlltoPatientName(string UserName);
        public Task<IEnumerable<Visit>> GetAlltoPatient(int id);
        public Task<IEnumerable<Visit>> GetAllToDoctor(int id);
    }
}
