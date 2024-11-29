using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class TimeProfile :Profile
    {
        public TimeProfile()
        {
            CreateMap<TimeViewModel,DoctorAvailability>();
        }
    }
}
