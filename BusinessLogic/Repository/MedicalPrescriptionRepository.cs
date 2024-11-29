using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class MedicalPrescriptionRepository : GenericRepository<MedicationPrescription>, IMedicalPrescriptionRepository
    {
        private readonly EHRdbContext _dbContext;

        public MedicalPrescriptionRepository(EHRdbContext dbContext) : base(dbContext)
        {
           _dbContext = dbContext;
        }

        public async Task<IEnumerable<MedicationPrescription>> GetAllPatientById(int id)
        {
            return await _dbContext.MedicationPrescriptions.Where(M => M.Patient.Id == id).Include(M=>M.Doctor.Staff).ToListAsync();
        }
        public async Task<IEnumerable<MedicationPrescription>> GetPrescriptionByPatientName(string Username)
        {
            return await _dbContext.MedicationPrescriptions.Where(V => V.Patient.Username == Username).Include(M => M.Patient).Include(M=>M.Doctor.Staff).ToListAsync();
        }
    }
}
