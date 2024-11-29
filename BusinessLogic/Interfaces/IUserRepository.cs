using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserRepository : IGenericRepository<Staff>
    {
        public Task<Staff> GetByUserNameAsync(string Username);
       
    }
}
