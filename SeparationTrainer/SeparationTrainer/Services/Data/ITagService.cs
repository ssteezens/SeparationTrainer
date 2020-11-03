using System.Threading.Tasks;
using SeparationTrainer.Models;

namespace SeparationTrainer.Services.Data
{
    public interface ITagService
    {
        Task<TagModel> AddAsync(TagModel tagModel);
    }
}