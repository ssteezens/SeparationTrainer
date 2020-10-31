using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeparationTrainer.Shared;

namespace SeparationTrainer.Data.Services
{
    public interface IActivityService
    {
        Task<ActivityModel> AddAsync(ActivityModel activity);

        IEnumerable<ActivityModel> GetAll();

        Task<IEnumerable<ActivityModel>> GetForDayAsync(DateTime day);
    }
}