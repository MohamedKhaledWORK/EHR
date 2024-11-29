using AutoMapper;
using DataAccess.Models;
using EHR.ViewModel;

namespace EHR.Profiles
{
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<MedicationPrescription, PrescriptionViewModel>().ReverseMap();
        }
    }
}
