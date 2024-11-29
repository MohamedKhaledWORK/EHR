using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace EHR.ViewModel
{
    public class LabViewModel
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public string LabTestType { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TestDate { get; set; }
        public string? TestResult { get; set; }
        public string? NormalRange { get; set; }
        public string TestStatus { get; set; }
    }
}
