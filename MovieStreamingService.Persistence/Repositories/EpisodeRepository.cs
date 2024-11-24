using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
{
    public EpisodeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Episode>> GetBySeasonIdAsync(int seasonId)
    {
        return await Context.Episodes
            .Where(episode => episode.SeasonId == seasonId)
            .ToListAsync();
    }
}