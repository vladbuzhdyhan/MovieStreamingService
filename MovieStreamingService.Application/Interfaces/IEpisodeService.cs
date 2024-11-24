using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface IEpisodeService : IService<Episode>
{
    Task<IEnumerable<Episode>> GetBySeasonIdAsync(int seasonId);
}