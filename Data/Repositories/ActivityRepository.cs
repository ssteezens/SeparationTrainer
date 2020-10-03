using System.Collections.Generic;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;
using SQLite;

namespace SeparationTrainer.Data.Repositories
{
    public class ActivityRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public ActivityRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Activity>().Wait();
        }

        public async Task<Activity> Add(Activity entity)
        {
            await _database.InsertAsync(entity);

            return entity;
        }

        public async Task<Activity> Update(Activity entity)
        {
            await _database.UpdateAsync(entity);

            return entity;
        }

        public async Task<Activity> Delete(Activity entity)
        {
            await _database.DeleteAsync(entity);

            return entity;
        }

        public async Task<Activity> Get(int id)
        {
            return await _database.GetAsync<Activity>(id);
        }

        public IEnumerable<Activity> GetAll()
        {
            return _database.Table<Activity>().ToListAsync().Result;
        }
    }
}
