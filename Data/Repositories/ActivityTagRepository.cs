using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public class ActivityTagRepository : RepositoryBase<ActivityTags>, IActivityTagRepository
    {
        public ActivityTagRepository(string dbPath) : base(dbPath)
        {
            Database.CreateTableAsync<ActivityTags>().Wait();
        }

        public override async Task<ActivityTags> GetAsync(int id)
        {
            return await Database.GetAsync<ActivityTags>(id);
        }

        public override async Task<IEnumerable<ActivityTags>> GetAllAsync()
        {
            return await Database.Table<ActivityTags>().ToListAsync();
        }

        public async Task<IEnumerable<ActivityTags>> GetByActivityId(int activityId)
        {
            var activityTags = await Database.Table<ActivityTags>().Where(tag => tag.ActivityId == activityId).ToListAsync();

            return activityTags;
        }
    }
}
