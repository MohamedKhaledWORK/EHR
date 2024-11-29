using DataAccess.Models;

namespace EHR.ViewModel
{
    public class DoctorViewModel
    {
            public int Id { get; set; }
            public int StaffId { get; set; }
            public Staff Staff { get; set; }
            public string Specialty { get; set; }
            public string Phone { get; set; }
            public string OfficeAddress { get; set; }
    }
}
