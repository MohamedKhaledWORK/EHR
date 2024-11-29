using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMedicalPrescriptionRepository : IGenericRepository<MedicationPrescription>
    {
        public Task<IEnumerable<MedicationPrescription>> GetAllPatientById(int id);
        public Task<IEnumerable<MedicationPrescription>> GetPrescriptionByPatientName(string Usename);

    }
}
