using AutoMapper;
using SeparationTrainer.Data.Entities;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<ActivityModel> UpdateAsync(ActivityModel activity)
        {
            var entity = _mapper.Map<Activity>(activity);
            var returnedEntity = await _activityRepository.UpdateAsync(entity);

            // remove current tags
            var tags = await _activityTagRepository.GetByActivityId(entity.Id);
            foreach (var tag in tags)
                await _activityTagRepository.DeleteAsync(tag);

            foreach (var tag in activity.Tags)
            {
                var tagEntity = _mapper.Map<ActivityTags>(tag);
                await _activityTagRepository.AddAsync(tagEntity);
            }

            return activity;
        }

        public async Task<ActivityModel> GetAsync(int id)
        {
            var activity = await _activityRepository.GetAsync(id);
            var model = _mapper.Map<ActivityModel>(activity);

            var activityTags = await _activityTagRepository.GetByActivityId(id);
            var activityTagModels = _mapper.Map<List<ActivityTagModel>>(activityTags);

            foreach (var activityTagModel in activityTagModels)
            {
                var tagDefinitionEntity = await _tagRepository.GetAsync(activityTagModel.TagId);
                var tagDefinitionModel = _mapper.Map<TagModel>(tagDefinitionEntity);

                activityTagModel.TagModel = tagDefinitionModel;
            }

            model.Tags = new ObservableCollection<ActivityTagModel>(activityTagModels);

            return model;
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
            var activityModels = _mapper.Map<List<ActivityModel>>(activityEntities);

            foreach (var activityModel in activityModels)
            {
                var activityTags = await _activityTagRepository.GetByActivityId(activityModel.Id);
                var activityTagModels = _mapper.Map<List<ActivityTagModel>>(activityTags);

                foreach (var activityTagModel in activityTagModels)
                {
                    var tagDefinitionEntity = await _tagRepository.GetAsync(activityTagModel.TagId);
                    var tagDefinitionModel = _mapper.Map<TagModel>(tagDefinitionEntity);

                    activityTagModel.TagModel = tagDefinitionModel;
                }

                activityModel.Tags = new ObservableCollection<ActivityTagModel>(activityTagModels);
            }

            return activityModels;
        }

        public async Task<IEnumerable<ActivityModel>> GetForDayAsync(DateTime day)
        {
            var activityEntities = await _activityRepository.GetForDayAsync(day);
            var activityModels = _mapper.Map<List<ActivityModel>>(activityEntities);

            foreach (var activityModel in activityModels)
            {
                var activityTags = await _activityTagRepository.GetByActivityId(activityModel.Id);
                var activityTagModels = _mapper.Map<List<ActivityTagModel>>(activityTags);

                foreach (var activityTagModel in activityTagModels)
                {
                    var tagDefinitionEntity = await _tagRepository.GetAsync(activityTagModel.TagId);
                    var tagDefinitionModel = _mapper.Map<TagModel>(tagDefinitionEntity);

                    activityTagModel.TagModel = tagDefinitionModel;
                }

                activityModel.Tags = new ObservableCollection<ActivityTagModel>(activityTagModels);
            }

            return activityModels;
        }
    }
}
