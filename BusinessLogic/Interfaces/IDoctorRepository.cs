﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IDoctorRepository :IGenericRepository<Doctors>
    {
        public Task<Doctors> GetDoctorByStaffID(int Id);

    }
}
