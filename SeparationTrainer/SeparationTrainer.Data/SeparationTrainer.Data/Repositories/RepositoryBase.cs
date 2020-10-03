using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        private readonly Context _context;

        protected RepositoryBase(Context context)
        {
            _context = context;
        }

        public virtual T Add(T entity)
        {
            var entry = _context.Add(entity);
            _context.SaveChanges();

            return (T) entry.Entity;
        }

        public virtual T Update(T entity)
        {
            var entry = _context.Update(entity);
            _context.SaveChanges();

            return (T) entry.Entity;
        }

        public virtual T Delete(T entity)
        {
            var entry = _context.Remove(entity);
            _context.SaveChanges();

            return (T) entry.Entity;
        }

        public virtual T Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
