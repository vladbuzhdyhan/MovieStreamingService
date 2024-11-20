using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface ICommentService : IService<Comment>
{
    Task<IEnumerable<Comment>> GetByEpisodeIdAsync(int episodeId);
}