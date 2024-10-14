using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class StatusRepository : Repository<Status>, IStatusRepository
{
    public StatusRepository(AppDbContext context) : base(context)
    {
    }
}