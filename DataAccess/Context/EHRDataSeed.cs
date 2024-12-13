using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class EHRDataSeed
    {

        public static async Task SeedAsync(EHRdbContext dbContext)
        {
            if (!dbContext.Staff.Any())
            {
                var StaffsData = File.ReadAllText("../DataAccess/Context/Data/Staff.json");
                var Staffs = JsonSerializer.Deserialize<List<Staff>>(StaffsData);

                if (Staffs?.Count > 0)
                {
                    foreach (var staff in Staffs)
                    {
                        await dbContext.Set<Staff>().AddAsync(staff);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.Doctors.Any())
            {
                var DoctorsData = File.ReadAllText("../DataAccess/Context/Data/Doctors.json");
                var Doctors = JsonSerializer.Deserialize<List<Doctors>>(DoctorsData);

                if (Doctors?.Count > 0)
                {
                    foreach (var Doctor in Doctors)
                    {
                        await dbContext.Set<Doctors>().AddAsync(Doctor);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.DoctorAvailabilities.Any())
            {
                var DoctorAvailabilitiesData = File.ReadAllText("../DataAccess/Context/Data/DoctorAvailabilities.json");
                var DoctorAvailability = JsonSerializer.Deserialize<List<DoctorAvailability>>(DoctorAvailabilitiesData);

                if (DoctorAvailability?.Count > 0)
                {
                    foreach (var DoctorA in DoctorAvailability)
                    {
                        await dbContext.Set<DoctorAvailability>().AddAsync(DoctorA);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.Patients.Any())
            {
                var PatientsData = File.ReadAllText("../DataAccess/Context/Data/Patients.json");
                var Patients = JsonSerializer.Deserialize<List<Patient>>(PatientsData);

                if (Patients?.Count > 0)
                {
                    foreach (var Patient in Patients)
                    {
                        await dbContext.Set<Patient>().AddAsync(Patient);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.Visits.Any())
            {
                var VisitsData = File.ReadAllText("../DataAccess/Context/Data/Visits.json");
                var Visits = JsonSerializer.Deserialize<List<Visit>>(VisitsData);

                if (Visits?.Count > 0)
                {
                    foreach (var Visit in Visits)
                    {
                        await dbContext.Set<Visit>().AddAsync(Visit);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.LabTests.Any())
            {
                var LabsData = File.ReadAllText("../DataAccess/Context/Data/Labs.json");
                var Labs = JsonSerializer.Deserialize<List<Lab>>(LabsData);

                if (Labs?.Count > 0)
                {
                    foreach (var Lab in Labs)
                    {
                        await dbContext.Set<Lab>().AddAsync(Lab);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.MedicationPrescriptions.Any())
            {
                var MedicationPrescriptionsData = File.ReadAllText("../DataAccess/Context/Data/MedicationPrescriptions.json");
                var MedicationPrescriptions = JsonSerializer.Deserialize<List<MedicationPrescription>>(MedicationPrescriptionsData);

                if (MedicationPrescriptions?.Count > 0)
                {
                    foreach (var MedicationPrescription in MedicationPrescriptions)
                    {
                        await dbContext.Set<MedicationPrescription>().AddAsync(MedicationPrescription);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }

        }
    }
}

