using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace EHR.ViewModel
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Doctors Doctor { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }
        public string? Diagnosis { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? FollowUpDate { get; set; } = DateTime.Now;
        public string? TreatmentPlan { get; set; }
        public string ReasonForVisit { get; set; }
        public string VisitType { get; set; }

    }
}
