using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Patient
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public bool HasInsurance { get; set; }

    }
}
