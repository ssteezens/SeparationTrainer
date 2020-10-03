using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>
    {
        private readonly Context _context;

        public ActivityRepository(Context context) : base(context)
        {
            _context = context;
        }

        public override Activity Get(int id)
        {
            return _context.Activities.Find(id);
        }
    }
}
