using DataAccess.Models;

namespace EHR.ViewModel
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }
        public string PatientUserName { get; set; }
        public string DoctorUserName { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public int FreqPerDay { get; set; }
        public string Duration {  get; set; }
        public string Comments {  get; set; }
    }
}
