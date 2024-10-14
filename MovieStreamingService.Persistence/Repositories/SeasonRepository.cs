using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class SeasonRepository : Repository<Season>, ISeasonRepository
{
    public SeasonRepository(AppDbContext context) : base(context)
    {
    }
}