using Microsoft.IdentityModel.Tokens;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class MovieService : Service<Movie>, IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository) : base(movieRepository)
    {
        _movieRepository = movieRepository;
    }
    public IEnumerable<Movie> SearchAsync(string? search, string? sortBy, bool? sortDesc, Dictionary<string, List<string>>? filters, int page, int pageSize)
    {
        IQueryable<Movie> query = search is not null ? _movieRepository.SearchAsync(search).Result.AsQueryable() : _movieRepository.GetAllAsync().Result.AsQueryable();

        if (sortBy is not null)
        {
            query = sortBy switch
            {
                "name" => sortDesc == true ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
                "imdbRating" => sortDesc == true ? query.OrderByDescending(m => m.ImdbRating) : query.OrderBy(m => m.ImdbRating),
                "firstAirDate" => sortDesc == true ? query.OrderByDescending(m => m.FirstAirDate) : query.OrderBy(m => m.FirstAirDate),
                "duration" => sortDesc == true ? query.OrderByDescending(m => m.Duration) : query.OrderBy(m => m.Duration),
                _ => query
            };
        }

        if (!filters.IsNullOrEmpty())
        {
            query = filters.Aggregate(query, (current, filter) => filter.Key switch
            {
                "type" => current.Where(m => filter.Value.Contains(m.Type.Name)),
                "status" => current.Where(m => filter.Value.Contains(m.Status.Name)),
                "country" => current.Where(m => m.Countries.Any(c => filter.Value.Contains(c.Name))),
                "tag" => current.Where(m => m.Tags.Any(t => filter.Value.Contains(t.Name))),
                "restrictedRating" => current.Where(m => filter.Value.Contains(m.RestrictedRating.ToString())),
                "imdbRating" => current.Where(m => m.ImdbRating > int.Parse(filter.Value.First()) && m.ImdbRating < int.Parse(filter.Value[1])),
                _ => current
            });
        }

        return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public async Task<IEnumerable<Movie>> GetByTagAsync(string tag)
    {
        return await _movieRepository.GetByTagAsync(tag);
    }

    public async Task<IEnumerable<Movie>> GetByTypeAsync(string type)
    {
        return await _movieRepository.GetByTypeAsync(type);
    }
}