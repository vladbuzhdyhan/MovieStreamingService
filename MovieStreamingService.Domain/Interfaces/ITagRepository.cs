using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<IEnumerable<Tag>> GetTagsAsync();
    Task<IEnumerable<Tag>> GetGenresAsync();
}