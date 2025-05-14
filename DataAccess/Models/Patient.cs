using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DataAccess.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain letters only.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain letters and numbers only.")]
        public string Username { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+@[a-z]+\.com$", ErrorMessage = "Email must be in the format user@example.com.")]
        public string Email { get; set; }

        [Range(15, 90, ErrorMessage = "Age must be between 15 and 90.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        public string MedicalHistory { get; set; }

        [Required(ErrorMessage = "Blood type is required.")]
        public string BloodType { get; set; }

        [Required(ErrorMessage = "Emergency contact is required.")]
        public string EmergencyContact { get; set; }

        [Required(ErrorMessage = "HasInsurance is required.")]
        public bool HasInsurance { get; set; }
    }
}
