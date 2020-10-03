using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public class SessionRepository : RepositoryBase<Session>
    {
        private readonly Context _context;

        public SessionRepository(Context context) : base(context)
        {
            _context = context;
        }

        public override Session Get(int id)
        {
            return _context.Sessions.Find(id);
        }
    }
}
