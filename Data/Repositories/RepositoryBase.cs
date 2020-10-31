using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;
using SQLite;

namespace SeparationTrainer.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        protected readonly SQLiteAsyncConnection Database;

        protected RepositoryBase(string dbPath)
        {
            Database = new SQLiteAsyncConnection(dbPath);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await Database.InsertAsync(entity);

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            await Database.UpdateAsync(entity);

            return entity;
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            await Database.DeleteAsync(entity);

            return entity;
        }

        public virtual Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}