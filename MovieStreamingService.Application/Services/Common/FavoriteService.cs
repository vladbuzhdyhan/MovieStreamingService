using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class FavoriteService : Service<Favourite>, IFavouriteService
{
    private readonly IFavouriteRepository _favoriteRepository;

    public FavoriteService(IFavouriteRepository favouriteRepository) : base(favouriteRepository)
    {
        _favoriteRepository = favouriteRepository;
    }
}