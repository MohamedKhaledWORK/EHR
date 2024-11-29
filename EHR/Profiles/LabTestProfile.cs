using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class LabTestProfile : Profile
    {
        public LabTestProfile()
        {
            CreateMap<Lab,LabViewModel>().ReverseMap();
            CreateMap<SimpleLabViewModel,Lab>().ReverseMap();
        }
    }
}
