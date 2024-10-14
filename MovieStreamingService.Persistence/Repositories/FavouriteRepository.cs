using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class FavouriteRepository : Repository<Favourite>, IFavouriteRepository
{
    public FavouriteRepository(AppDbContext context) : base(context)
    {
    }
}