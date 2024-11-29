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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EHRdbContext _dbContext;

        public GenericRepository(EHRdbContext dbContext) 
        {
           _dbContext = dbContext;
        }
        public async Task AddAsync(T Item)
        {
           await _dbContext.AddAsync(Item);
        }

        public void Delete(T Item)
        {
            _dbContext.Remove(Item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Doctors)) return (IEnumerable<T>) await _dbContext.Doctors.Include(d=>d.Staff).Include(D=>D.Availabilities).ToListAsync();
            if(typeof(T)==typeof(Lab)) return (IEnumerable<T>) await _dbContext.LabTests.Include(L=>L.Patient).ToListAsync();
            if(typeof(T)==typeof(Staff)) return (IEnumerable<T>) await _dbContext.Staff.Where(S=>S.Role!="Admin").ToListAsync();
            if(typeof(T)==typeof(Visit)) return (IEnumerable<T>) await _dbContext.Visits.Include(V=>V.Doctor.Staff).Include(V=>V.Patient).ToListAsync();
            if(typeof(T)==typeof(MedicationPrescription)) return (IEnumerable<T>) await _dbContext.MedicationPrescriptions.Include(V=>V.Patient).Include(V=>V.Doctor).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int Id)
        {
            if (typeof(T) == typeof(Visit)) return await _dbContext.Visits.Include(V => V.Doctor.Staff).Include(V => V.Patient).FirstOrDefaultAsync(V=>V.Id==Id) as T;
            if (typeof(T) == typeof(Lab)) return await _dbContext.LabTests.Include(L => L.Patient).FirstOrDefaultAsync(L=>L.Id==Id) as T;
            if (typeof(T) == typeof(Doctors)) return await _dbContext.Doctors.Include(d => d.Staff).Include(D => D.Availabilities).FirstOrDefaultAsync(D=>D.Id==Id) as T;

            return await _dbContext.Set<T>().FindAsync(Id);
        }
    
        public void Update(T Item)
        {
           _dbContext.Update(Item);
        }

        public Task<int> CompleteAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
