using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;

namespace MovieStreamingService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search(string? search, string? sortBy, bool? sortDesc, Dictionary<string, List<string>>? filters, int page, int pageSize)
        {
            IEnumerable<Movie> movies;
            try
            {
                movies = _movieService.SearchAsync(search, sortBy, sortDesc, filters, page, pageSize);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(movies.Select(movie => new MovieDto(
                movie.Id,
                movie.Name,
                movie.Description,
                movie.RestrictedRating.ToString(),
                movie.Poster,
                movie.Duration,
                movie.FirstAirDate,
                movie.LastAirDate,
                movie.AmountOfEpisodes,
                movie.ImdbRating,
                movie.Background
            )));
        }
    }
}
