using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface IMovieService : IService<Movie>
{
    IEnumerable<Movie> SearchAsync(string? search, string? sortBy, bool? sortDesc, Dictionary<string, List<string>>? filters, int page, int pageSize);
    Task<IEnumerable<Movie>> GetByTagAsync(string tag);
    Task<IEnumerable<Movie>> GetByTypeAsync(string type);
}