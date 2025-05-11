using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Doctors
    {
        public int Id { get; set; } 
        public int StaffId { get; set; }
        public Staff Staff { get; set; }
        [Required(ErrorMessage = "Specialty is Required")]
        public string Specialty { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits.")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "OfficeAddress is Required")]
        public string OfficeAddress { get; set; }
        public ICollection<DoctorAvailability> Availabilities { get; set; }= new HashSet<DoctorAvailability>();


    }
}
