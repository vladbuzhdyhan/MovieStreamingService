using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class FavouriteService : Service<Favourite>, IFavouriteService
{
    private readonly IFavouriteRepository _favoriteRepository;

    public FavouriteService(IFavouriteRepository favouriteRepository) : base(favouriteRepository)
    {
        _favoriteRepository = favouriteRepository;
    }

    public async Task<IEnumerable<Favourite>> GetByUserIdAsync(Guid userId)
    {
        return await _favoriteRepository.GetByUserIdAsync(userId);
    }
}