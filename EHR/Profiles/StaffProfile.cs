using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class StaffProfile: Profile
    {
        public StaffProfile() 
        {
            CreateMap<Staff,StaffViewModel>();
        }
    }
}
