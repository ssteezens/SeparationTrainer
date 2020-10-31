using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public class ActivityTagRepository : RepositoryBase<ActivityTags>
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
    }
}
