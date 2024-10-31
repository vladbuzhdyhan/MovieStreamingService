using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface IFavouriteRepository : IRepository<Favourite>
{
    Task<IEnumerable<Favourite>> GetByUserIdAsync(Guid userId);
}