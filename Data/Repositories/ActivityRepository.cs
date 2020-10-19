using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            var activities = await _database.Table<Activity>().ToListAsync();

            await _database.Table<Activity>().DeleteAsync(i => i.ElapsedTime == TimeSpan.MinValue);

            return activities;
        }

        public async Task<IEnumerable<Activity>> GetForDayAsync(DateTime day)
        {
            // todo: convert db to use tickets so query can be more efficient
            var thing = await _database.Table<Activity>().ToListAsync();
            var activities = thing.Where(i => i.Created.Date == day.Date).ToList();

            return activities;
        }
    }
}
