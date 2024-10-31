using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface IFavouriteService : IService<Favourite>
{
    Task<IEnumerable<Favourite>> GetByUserIdAsync(Guid userId);
}