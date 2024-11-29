using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidatePatient(string username, string password);
        Task<bool> ValidateStaff(string username, string password);
        Task<bool> RegisterPatient(Patient model);
    }
}
