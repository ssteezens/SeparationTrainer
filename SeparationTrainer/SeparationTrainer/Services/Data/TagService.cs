using System;
using System.Threading.Tasks;
using AutoMapper;
using SeparationTrainer.Data.Entities;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;

namespace SeparationTrainer.Services.Data
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly TagRepository _repository;

        public TagService(TagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TagModel> AddAsync(TagModel tagModel)
        {
            var entity = _mapper.Map<Tag>(tagModel);
            var returnedEntity = await _repository.AddAsync(entity);

            tagModel.Id = returnedEntity.Id;

            return tagModel;
        }
    }
}