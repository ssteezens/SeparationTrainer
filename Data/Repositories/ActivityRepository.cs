using SeparationTrainer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeparationTrainer.Data.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(string dbPath) : base(dbPath)
        {
            Database.CreateTableAsync<Activity>().Wait();
        }

        public IEnumerable<Activity> GetAll()
        {
            return Database.Table<Activity>().ToListAsync().Result;
        }

        public override async Task<IEnumerable<Activity>> GetAllAsync()
        {
            var activities = await Database.Table<Activity>().ToListAsync();

            return activities;
        }

        public async Task<IEnumerable<Activity>> GetForDayAsync(DateTime day)
        {
            // todo: convert db to use tickets so query can be more efficient
            var thing = await Database.Table<Activity>().ToListAsync();
            var activities = thing.Where(i => i.Created.Date == day.Date).ToList();

            return activities;
        }
    }
}
