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

        public override async Task<Tag> AddAsync(Tag entity)
        {
            // find if tag already exists in database, if so return it instead of creating a duplicate
            var existingTag = await Database.Table<Tag>().FirstOrDefaultAsync(tag => tag.Name == entity.Name);

            if (existingTag != null)
                return existingTag;

            return await base.AddAsync(entity);
        }

        public override async Task<Tag> GetAsync(int id)
        {
            return await Database.Table<Tag>().FirstOrDefaultAsync(tag => tag.Id == id);
        }

        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await Database.Table<Tag>().ToListAsync();
        }
    }
}
