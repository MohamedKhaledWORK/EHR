using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Visit
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [ForeignKey(nameof(Doctors))]
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public string Date {  get; set; }
        public string? Diagnosis {  get; set; }
        public DateTime? FollowUpDate {  get; set; }
        public string? TreatmentPlan {  get; set; }
        public string ReasonForVisit {  get; set; }
        public string VisitType {  get; set; }

    }
}
