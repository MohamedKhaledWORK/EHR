using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class PatientProfile :Profile
    {
        public PatientProfile()
        {
             CreateMap<Patient,PatientViewModel>().ReverseMap();
        }
    }
}
