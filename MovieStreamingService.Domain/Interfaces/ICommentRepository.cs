using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByEpisodeIdAsync(int episodeId);
}