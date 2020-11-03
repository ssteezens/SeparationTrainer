using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeparationTrainer.Models;

namespace SeparationTrainer.Services.Data
{
    public interface IActivityService
    {
        Task<ActivityModel> AddAsync(ActivityModel activity);
        IEnumerable<ActivityModel> GetAll();
        Task<IEnumerable<ActivityModel>> GetAllAsync();
        Task<IEnumerable<ActivityModel>> GetForDayAsync(DateTime day);
    }
}