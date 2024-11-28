using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class SeasonRepository : Repository<Season>, ISeasonRepository
{
    public SeasonRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Season>> GetByMovieIdAsync(int movieId)
    {
        return await Context.Seasons
            .Where(s => s.MovieId == movieId)
            .ToListAsync();
    }
}