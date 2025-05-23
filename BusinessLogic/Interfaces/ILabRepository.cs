﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ILabRepository : IGenericRepository<Lab>
    {
        public Task<IEnumerable<Lab>> GetLabByPatientNameAsync(string Username);

    }
}
