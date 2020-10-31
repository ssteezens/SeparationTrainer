using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public class TagRepository : RepositoryBase<Tag>
    {
        public TagRepository(string dbPath) : base(dbPath)
        {
            Database.CreateTableAsync<Tag>().Wait();
        }

        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await Database.Table<Tag>().ToListAsync();
        }
    }
}
