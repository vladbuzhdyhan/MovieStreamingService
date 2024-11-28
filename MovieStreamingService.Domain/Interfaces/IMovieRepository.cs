using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface IMovieRepository : IRepository<Movie>
{
    Task<IEnumerable<Movie>> SearchAsync(string search);
    Task<IEnumerable<Movie>> GetByTagAsync(string tag);
    Task<IEnumerable<Movie>> GetByTypeAsync(string type);
}