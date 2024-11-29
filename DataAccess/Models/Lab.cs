using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Lab
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string LabTestType { get; set; }
        public DateTime TestDate { get; set; }
        public string? TestResult { get; set; }
        public string? NormalRange { get; set; }
        public string TestStatus { get; set; }

    }
}
