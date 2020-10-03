using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Data.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        T Get(int id);
    }
}
