using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface ISeasonService : IService<Season>
{
    Task<IEnumerable<Season>> GetByMovieIdAsync(int movieId);
}