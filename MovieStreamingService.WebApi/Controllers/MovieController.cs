using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Enums;
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
                Tags = movie.Tags.Select(tag => tag.Name).ToList(),
                People = movie.People.Select(person => new PersonDto
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    BirthDate = person.BirthDate,
                    DeathDate = person.DeathDate,
                    Image = PersonController.GetImageFormFile(person.Image),
                    Biography = person.Biography
                }).ToList()
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

        [HttpGet]
        [Route("tag/{tag}")]
        public IActionResult GetByTag(string tag)
        {
            var movies = _movieService.GetByTagAsync(tag).Result;

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
        [Route("type/{type}")]
        public IActionResult GetByType(string type)
        {
            var movies = _movieService.GetByTypeAsync(type).Result;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MovieDto movieDto)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var fileExtension = Path.GetExtension(movieDto.Poster.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.Poster.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            fileExtension = Path.GetExtension(movieDto.Background.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.Background.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            fileExtension = Path.GetExtension(movieDto.BigPoster.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.BigPoster.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            fileExtension = Path.GetExtension(movieDto.ImageTitle.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.ImageTitle.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            var movie = new Movie
            {
                Name = movieDto.Name,
                Description = movieDto.Description,
                RestrictedRating = (RestrictedRating) Enum.Parse(typeof(RestrictedRating), movieDto.RestrictedRating),
                Duration = movieDto.Duration,
                FirstAirDate = movieDto.FirstAirDate,
                LastAirDate = movieDto.LastAirDate,
                AmountOfEpisodes = movieDto.AmountOfEpisodes,
                ImdbRating = movieDto.ImdbRating,
                Countries = movieDto.Countries.Select(country => new Country { Name = country }).ToList(),
                Tags = movieDto.Tags.Select(tag => new Tag { Name = tag }).ToList()
            };

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.Poster.FileName).ToLower()}";
            var filePath = Path.Combine("wwwroot/posters", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.Poster.CopyToAsync(fileStream);
            }
            movie.Poster = fileName;

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.Background.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/backgrounds", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.Background.CopyToAsync(fileStream);
            }
            movie.Background = fileName;

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.BigPoster.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/bigPosters", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.BigPoster.CopyToAsync(fileStream);
            }
            movie.BigPoster = fileName;

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.ImageTitle.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/imageTitles", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.ImageTitle.CopyToAsync(fileStream);
            }
            movie.ImageTitle = fileName;

            try
            {
                await _movieService.AddAsync(movie);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MovieDto movieDto)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var fileExtension = Path.GetExtension(movieDto.Poster.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.Poster.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            fileExtension = Path.GetExtension(movieDto.Background.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.Background.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            fileExtension = Path.GetExtension(movieDto.BigPoster.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.BigPoster.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            fileExtension = Path.GetExtension(movieDto.ImageTitle.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
            if (movieDto.ImageTitle.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");

            var movie = new Movie
            {
                Id = id,
                Name = movieDto.Name,
                Description = movieDto.Description,
                RestrictedRating = (RestrictedRating)Enum.Parse(typeof(RestrictedRating), movieDto.RestrictedRating),
                Poster = movieDto.Poster.FileName,
                Duration = movieDto.Duration,
                FirstAirDate = movieDto.FirstAirDate,
                LastAirDate = movieDto.LastAirDate,
                AmountOfEpisodes = movieDto.AmountOfEpisodes,
                ImdbRating = movieDto.ImdbRating,
                Background = movieDto.Background.FileName,
                BigPoster = movieDto.BigPoster.FileName,
                ImageTitle = movieDto.ImageTitle.FileName,
                Countries = movieDto.Countries.Select(country => new Country { Name = country }).ToList(),
                Tags = movieDto.Tags.Select(tag => new Tag { Name = tag }).ToList()
            };

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.Poster.FileName).ToLower()}";
            var filePath = Path.Combine("wwwroot/posters", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.Poster.CopyToAsync(fileStream);
            }
            System.IO.File.Delete(Path.Combine("wwwroot/posters", movie.Poster));
            movie.Poster = fileName;

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.Background.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/backgrounds", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.Background.CopyToAsync(fileStream);
            }
            System.IO.File.Delete(Path.Combine("wwwroot/backgrounds", movie.Background));
            movie.Background = fileName;

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.BigPoster.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/bigPosters", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.BigPoster.CopyToAsync(fileStream);
            }
            System.IO.File.Delete(Path.Combine("wwwroot/bigPosters", movie.BigPoster));
            movie.BigPoster = fileName;

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(movieDto.ImageTitle.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/imageTitles", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await movieDto.ImageTitle.CopyToAsync(fileStream);
            }
            System.IO.File.Delete(Path.Combine("wwwroot/imageTitles", movie.ImageTitle));
            movie.ImageTitle = fileName;

            try
            {
                await _movieService.UpdateAsync(movie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
                return NotFound();

            System.IO.File.Delete(Path.Combine("wwwroot/posters", movie.Poster));
            System.IO.File.Delete(Path.Combine("wwwroot/backgrounds", movie.Background));
            System.IO.File.Delete(Path.Combine("wwwroot/bigPosters", movie.BigPoster));
            System.IO.File.Delete(Path.Combine("wwwroot/imageTitles", movie.ImageTitle));

            try
            {
                await _movieService.DeleteAsync(movie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
