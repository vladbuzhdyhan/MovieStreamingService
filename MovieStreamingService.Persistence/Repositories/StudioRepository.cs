using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class StudioRepository : Repository<Studio>, IStudioRepository
{
    public StudioRepository(AppDbContext context) : base(context)
    {
    }
}