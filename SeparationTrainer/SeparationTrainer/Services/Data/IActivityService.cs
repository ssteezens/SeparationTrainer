using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeparationTrainer.Models;

namespace SeparationTrainer.Services.Data
{
    public interface IActivityService
    {
        Task<ActivityModel> AddAsync(ActivityModel activity);
        Task<ActivityModel> UpdateAsync(ActivityModel activity);
        Task<ActivityModel> GetAsync(int id);
        Task<ActivityModel> DeleteAsync(int id);
        IEnumerable<ActivityModel> GetAll();
        Task<IEnumerable<ActivityModel>> GetAllAsync();
        Task<IEnumerable<ActivityModel>> GetForDayAsync(DateTime day);
    }
}