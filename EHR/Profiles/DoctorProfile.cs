using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class DoctorProfile :Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctors,DoctorViewModel> ();
        }
    }
}
