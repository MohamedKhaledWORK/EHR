using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class DoctorAvailability
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public string DayOfWeek { get; set; }
        public string TimeSlot { get; set; }
    }
}
