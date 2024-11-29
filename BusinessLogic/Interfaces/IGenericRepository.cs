using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int Id);
        public Task AddAsync(T Item);
        public void Update(T Item);
        public void Delete(T Item);
        public Task<int> CompleteAsync();
    }
}
