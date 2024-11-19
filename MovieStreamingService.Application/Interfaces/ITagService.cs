using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface ITagService : IService<Tag>
{
    Task<IEnumerable<Tag>> GetTagsAsync();
    Task<IEnumerable<Tag>> GetGenresAsync();
}