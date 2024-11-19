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

        public static FormFile GetPosterFormFile(string? posterPath)
        {
            if (string.IsNullOrEmpty(posterPath))
                return null;

            var memoryStream = new MemoryStream();
            try
            {
                using var stream = new FileStream(Path.Combine("wwwroot/posters", posterPath), FileMode.Open, FileAccess.Read);
                stream.CopyTo(memoryStream);
            }
            catch
            {
                return null;
            }

            return new FormFile(memoryStream, 0, memoryStream.Length, "poster", posterPath)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + Path.GetExtension(posterPath).ToLower().Remove(0, 1),
                ContentDisposition = $"form-data; name=poster; filename={posterPath}"
            };
        }

        public static FormFile GetBackgroundFormFile(string? backgroundPath)
        {
            if (string.IsNullOrEmpty(backgroundPath))
                return null;

            var memoryStream = new MemoryStream();
            try
            {
                using var stream = new FileStream(Path.Combine("wwwroot/backgrounds", backgroundPath), FileMode.Open, FileAccess.Read);
                stream.CopyTo(memoryStream);
            }
            catch
            {
                return null;
            }

            return new FormFile(memoryStream, 0, memoryStream.Length, "background", backgroundPath)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + Path.GetExtension(backgroundPath).ToLower().Remove(0, 1),
                ContentDisposition = $"form-data; name=background; filename={backgroundPath}"
            };
        }

        public static FormFile GetBigPosterFormFile(string? bigPosterPath)
        {
            if (string.IsNullOrEmpty(bigPosterPath))
                return null;

            var memoryStream = new MemoryStream();
            try
            {
                using var stream = new FileStream(Path.Combine("wwwroot/bigPosters", bigPosterPath), FileMode.Open, FileAccess.Read);
                stream.CopyTo(memoryStream);
            }
            catch
            {
                return null;
            }

            return new FormFile(memoryStream, 0, memoryStream.Length, "bigPoster", bigPosterPath)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + Path.GetExtension(bigPosterPath).ToLower().Remove(0, 1),
                ContentDisposition = $"form-data; name=bigPoster; filename={bigPosterPath}"
            };
        }

        public static FormFile GetImageTitleFormFile(string? imageTitlePath)
        {
            if (string.IsNullOrEmpty(imageTitlePath))
                return null;

            var memoryStream = new MemoryStream();
            try
            {
                using var stream = new FileStream(Path.Combine("wwwroot/imageTitles", imageTitlePath), FileMode.Open, FileAccess.Read);
                stream.CopyTo(memoryStream);
            }
            catch
            {
                return null;
            }

            return new FormFile(memoryStream, 0, memoryStream.Length, "imageTitle", imageTitlePath)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + Path.GetExtension(imageTitlePath).ToLower().Remove(0, 1),
                ContentDisposition = $"form-data; name=imageTitle; filename={imageTitlePath}"
            };
        }

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var movies = _movieService.GetAllAsync().Result;

            return Ok(movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                RestrictedRating = movie.RestrictedRating.ToString(),
                Poster = GetPosterFormFile(movie.Poster),
                Duration = movie.Duration,
                FirstAirDate = movie.FirstAirDate,
                LastAirDate = movie.LastAirDate,
                AmountOfEpisodes = movie.AmountOfEpisodes,
                ImdbRating = movie.ImdbRating,
                Background = GetBackgroundFormFile(movie.Background),
                BigPoster = GetBigPosterFormFile(movie.BigPoster),
                ImageTitle = GetImageTitleFormFile(movie.ImageTitle),
                Countries = movie.Countries.Select(country => country.Name).ToList(),
                Tags = movie.Tags.Select(tag => tag.Name).ToList()
            }));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var movie = _movieService.GetByIdAsync(id).Result;
            if (movie == null)
                return NotFound();

            return Ok(new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                RestrictedRating = movie.RestrictedRating.ToString(),
                Poster = GetPosterFormFile(movie.Poster),
                Duration = movie.Duration,
                FirstAirDate = movie.FirstAirDate,
                LastAirDate = movie.LastAirDate,
                AmountOfEpisodes = movie.AmountOfEpisodes,
                ImdbRating = movie.ImdbRating,
                Background = GetBackgroundFormFile(movie.Background),
                BigPoster = GetBigPosterFormFile(movie.BigPoster),
                ImageTitle = GetImageTitleFormFile(movie.ImageTitle),
                Countries = movie.Countries.Select(country => country.Name).ToList(),
                Tags = movie.Tags.Select(tag => tag.Name).ToList()
            });
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

            return Ok(movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                RestrictedRating = movie.RestrictedRating.ToString(),
                Poster = GetPosterFormFile(movie.Poster),
                Duration = movie.Duration,
                FirstAirDate = movie.FirstAirDate,
                LastAirDate = movie.LastAirDate,
                AmountOfEpisodes = movie.AmountOfEpisodes,
                ImdbRating = movie.ImdbRating,
                Background = GetBackgroundFormFile(movie.Background),
                BigPoster = GetBigPosterFormFile(movie.BigPoster),
                ImageTitle = GetImageTitleFormFile(movie.ImageTitle),
                Countries = movie.Countries.Select(country => country.Name).ToList(),
                Tags = movie.Tags.Select(tag => tag.Name).ToList()
            }
            ));
        }
    }
}
