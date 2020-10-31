using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public interface IActivityRepository : IRepository<Activity>
    {
        IEnumerable<Activity> GetAll();

        Task<IEnumerable<Activity>> GetForDayAsync(DateTime day);
    }
}
