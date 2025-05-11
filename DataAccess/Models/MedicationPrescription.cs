using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class MedicationPrescription
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [ForeignKey(nameof(Doctors))]
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public int FreqPerDay { get; set; }
        public string Duration {  get; set; }
        public string? Comments {  get; set; }

    }
}
