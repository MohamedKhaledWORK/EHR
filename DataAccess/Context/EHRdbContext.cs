using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class EHRdbContext : DbContext
    {
        public EHRdbContext(DbContextOptions<EHRdbContext> options): base(options)
        {
            
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<MedicationPrescription> MedicationPrescriptions { get; set; }
        public DbSet<Lab> LabTests { get; set; }
        public DbSet<Staff> Staff { get; set; }
    }
}
