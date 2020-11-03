using System.Collections.Generic;
using System.Threading.Tasks;
using SeparationTrainer.Data.Entities;

namespace SeparationTrainer.Data.Repositories
{
    public interface IActivityTagRepository 
    {
        Task<IEnumerable<ActivityTags>> GetByActivityId(int activityId);
    }
}