using System.ComponentModel.DataAnnotations;

namespace EHR.ViewModel
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "MedicalHistory is Required")]
        public string MedicalHistory { get; set; }
        public string? BloodType { get; set; }
        [Required(ErrorMessage = "EmergencyContact is Required")]
        public string EmergencyContact { get; set; }
        public bool HasInsurance { get; set; }
    }
}
