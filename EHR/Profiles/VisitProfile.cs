using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class VisitProfile :Profile
    {
        public VisitProfile()
        {
            CreateMap<Visit,VisitViewModel>().ReverseMap();
        }
    }
}
