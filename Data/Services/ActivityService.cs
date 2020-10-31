using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SeparationTrainer.Data.Entities;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Shared;

namespace SeparationTrainer.Data.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ActivityRepository _activityRepository;
        private readonly ActivityTagRepository _activityTagRepository;
        private readonly IMapper _mapper;

        public ActivityService(ActivityRepository activityRepository,
            ActivityTagRepository activityTagRepository,
            IMapper mapper)
        {
            _activityRepository = activityRepository;
            _activityTagRepository = activityTagRepository;
            _mapper = mapper;
        }

        public async Task<ActivityModel> AddAsync(ActivityModel activity)
        {
            var entity = _mapper.Map<Activity>(activity);

            await _activityRepository.AddAsync(entity);

            foreach (var tag in activity.Tags)
            {
                var activityTag = new ActivityTags()
                {
                    TagId = tag.Tag.Id,
                    ActivityId = activity.Id,
                    AppliedOn = tag.AppliedOn
                };

                await _activityTagRepository.AddAsync(activityTag);
            }

            return activity;
        }

        public IEnumerable<ActivityModel> GetAll()
        {
            var entities = _activityRepository.GetAllAsync();
            var models = _mapper.Map<List<ActivityModel>>(entities);

            return models;
        }

        public async Task<IEnumerable<ActivityModel>> GetForDayAsync(DateTime day)
        {
            var entities = await _activityRepository.GetForDayAsync(day);
            var models = _mapper.Map<List<ActivityModel>>(entities);

            return models;
        }
    }
}
