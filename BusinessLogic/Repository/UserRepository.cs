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
    public class UserRepository : GenericRepository<Staff> , IUserRepository
    {
        private readonly EHRdbContext _dbContext;

        public UserRepository(EHRdbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Staff> GetByUserNameAsync(string username) => await _dbContext.Staff.Where(S => S.UserName == username).FirstOrDefaultAsync();
        
    }
}
