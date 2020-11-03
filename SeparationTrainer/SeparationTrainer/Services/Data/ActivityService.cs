using AutoMapper;
using SeparationTrainer.Data.Entities;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeparationTrainer.Services.Data
{
    public class ActivityService : IActivityService
    {
        private readonly ActivityRepository _activityRepository;
        private readonly ActivityTagRepository _activityTagRepository;
        private readonly TagRepository _tagRepository;
        private readonly IMapper _mapper;

        public ActivityService(ActivityRepository activityRepository,
            ActivityTagRepository activityTagRepository,
            TagRepository tagRepository,
            IMapper mapper)
        {
            _activityRepository = activityRepository;
            _activityTagRepository = activityTagRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<ActivityModel> AddAsync(ActivityModel activity)
        {
            var entity = _mapper.Map<Activity>(activity);

            var returnedEntity = await _activityRepository.AddAsync(entity);

            activity.Id = returnedEntity.Id;

            foreach (var tag in activity.Tags)
            {
                var activityTag = new ActivityTags()
                {
                    TagId = tag.TagModel.Id,
                    ActivityId = activity.Id,
                    AppliedOn = tag.AppliedOn
                };

                await _activityTagRepository.AddAsync(activityTag);
            }

            return activity;
        }

        public IEnumerable<ActivityModel> GetAll()
        {
            var activityEntities = _activityRepository.GetAllAsync();
            var models = _mapper.Map<List<ActivityModel>>(activityEntities);

            return models;
        }

        public async Task<IEnumerable<ActivityModel>> GetAllAsync()
        {
            var activityEntities = await _activityRepository.GetAllAsync();
            var activityModels = new List<ActivityModel>();

            foreach (var activityEntity in activityEntities)
            {
                var model = _mapper.Map<ActivityModel>(activityEntity);
                var activityTags = await _activityTagRepository.GetByActivityId(activityEntity.Id);

                model.Tags = new List<ActivityTagModel>();
                foreach (var activityTag in activityTags)
                {
                    var tag = await _tagRepository.GetAsync(activityTag.TagId);
                    var tagModel = _mapper.Map<TagModel>(tag);

                    var activityTagModel = _mapper.Map<ActivityTagModel>(activityTag);
                    activityTagModel.TagModel = tagModel;

                    model.Tags.Add(activityTagModel);
                }
                activityModels.Add(model);
            }

            return activityModels;
        }

        public async Task<IEnumerable<ActivityModel>> GetForDayAsync(DateTime day)
        {
            var activityEntities = await _activityRepository.GetForDayAsync(day);
            var activityModels = new List<ActivityModel>();

            foreach (var activityEntity in activityEntities)
            {
                var model = _mapper.Map<ActivityModel>(activityEntity);
                var activityTags = await _activityTagRepository.GetByActivityId(activityEntity.Id);

                model.Tags = new List<ActivityTagModel>();
                foreach (var activityTag in activityTags)
                {
                    var tag = await _tagRepository.GetAsync(activityTag.TagId);
                    var tagModel = _mapper.Map<TagModel>(tag);

                    var activityTagModel = _mapper.Map<ActivityTagModel>(activityTag);
                    activityTagModel.TagModel = tagModel;

                    model.Tags.Add(activityTagModel);
                }
                activityModels.Add(model);
            }

            return activityModels;
        }
    }
}
