using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Data.Repositories
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);

       Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);

        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
    }
}
