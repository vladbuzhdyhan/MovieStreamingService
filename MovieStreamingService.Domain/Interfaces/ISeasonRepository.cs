using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface ISeasonRepository : IRepository<Season>
{
    Task<IEnumerable<Season>> GetByMovieIdAsync(int movieId);
}