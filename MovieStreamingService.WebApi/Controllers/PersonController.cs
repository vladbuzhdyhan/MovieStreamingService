using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;
using System.Linq;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public static FormFile? GetImageFormFile(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return null;

        var memoryStream = new MemoryStream();
        try
        {
            using var stream =
                new FileStream(Path.Combine("wwwroot/person", imagePath), FileMode.Open, FileAccess.Read);
            stream.CopyTo(memoryStream);
        }
        catch
        {
            return null;
        }

        return new FormFile(memoryStream, 0, memoryStream.Length, "person", imagePath)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/" + Path.GetExtension(imagePath).ToLower().Remove(0, 1),
            ContentDisposition = $"form-data; name=person; filename={imagePath}"
        };
    }

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var persons = _personService.GetAllAsync().Result;
        return Ok(persons.Select(person => new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            BirthDate = person.BirthDate,
            DeathDate = person.DeathDate,
            Image = GetImageFormFile(person.Image),
            Biography = person.Biography
        }));
    }

    [HttpGet("page-{page}&pageSize-{pageSize}")]
    public IActionResult GetPage(int page, int pageSize)
    {
        var persons = _personService.GetByPageAsync(page, pageSize).Result;

        return Ok(persons.Select(person => new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            BirthDate = person.BirthDate,
            DeathDate = person.DeathDate,
            Image = GetImageFormFile(person.Image),
            Biography = person.Biography
        }));
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var person = _personService.GetByIdAsync(id).Result;
        if (person == null)
            return NotFound();

        return Ok(new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            BirthDate = person.BirthDate,
            DeathDate = person.DeathDate,
            Image = GetImageFormFile(person.Image),
            Biography = person.Biography,
            Movies = person.Movies.Select(
                movie => new MovieDto
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Description = movie.Description,
                    RestrictedRating = movie.RestrictedRating.ToString(),
                    Poster = MovieController.GetPosterFormFile(movie.Poster),
                    Duration = movie.Duration,
                    FirstAirDate = movie.FirstAirDate,
                    LastAirDate = movie.LastAirDate,
                    AmountOfEpisodes = movie.AmountOfEpisodes,
                    ImdbRating = movie.ImdbRating,
                    Background = MovieController.GetBackgroundFormFile(movie.Background),
                    BigPoster = MovieController.GetBigPosterFormFile(movie.BigPoster),
                    ImageTitle = MovieController.GetImageTitleFormFile(movie.ImageTitle),
                    Countries = movie.Countries.Select(country => country.Name).ToList(),
                    Tags = movie.Tags.Select(tag => tag.Name).ToList()
                }
            ).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] PersonDto personDto)
    {
        if (personDto.Image != null)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(personDto.Image.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");

            if (personDto.Image.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");
        }

        var person = new Person
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            BirthDate = personDto.BirthDate,
            DeathDate = personDto.DeathDate,
            Biography = personDto.Biography
        };

        if (personDto.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(personDto.Image.FileName).ToLower()}";
            var filePath = Path.Combine("wwwroot/person", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await personDto.Image.CopyToAsync(fileStream);
            }

            person.Image = fileName;
        }
        else
            person.Image = null;

        try
        {
            await _personService.AddAsync(person);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromForm] PersonDto personDto)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null)
            return NotFound();

        person.FirstName = personDto.FirstName;
        person.LastName = personDto.LastName;
        person.BirthDate = personDto.BirthDate;
        person.DeathDate = personDto.DeathDate;
        person.Biography = personDto.Biography;

        if (personDto.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(personDto.Image.FileName).ToLower()}";
            var filePath = Path.Combine("wwwroot/person", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await personDto.Image.CopyToAsync(fileStream);
            }

            person.Image = fileName;
        }
        else if (person.Image != null && personDto.Image == null)
        {
            System.IO.File.Delete(Path.Combine("wwwroot/person", person.Image));
            person.Image = null;
        }

        try
        {
            await _personService.UpdateAsync(person);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null)
            return NotFound();

        await _personService.DeleteAsync(person);
        if (person.Image != null) System.IO.File.Delete(Path.Combine("wwwroot/person", person.Image));

        return Ok();
    }
}