using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class FavouriteRepository : Repository<Favourite>, IFavouriteRepository
{
    public FavouriteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Favourite>> GetByUserIdAsync(Guid userId)
    {
        return await Context.Favourites.Where(f => f.UserId == userId)
            .OrderByDescending(f => f.AddDate)
            .Include(f => f.Movie)
            .ToListAsync();
    }
}