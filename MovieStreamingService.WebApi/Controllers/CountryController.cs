using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var countries = _countryService.GetAllAsync().Result;
        return Ok(countries.Select(country => new CountryDto
        {
            Id = country.Id,
            Name = country.Name
        }));
    }

    [HttpGet("page-{page}&pageSize-{pageSize}")]
    public IActionResult GetPage(int page, int pageSize)
    {
        var countries = _countryService.GetByPageAsync(page, pageSize).Result;
        return Ok(countries.Select(country => new CountryDto
        {
            Id = country.Id,
            Name = country.Name
        }));
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var country = _countryService.GetByIdAsync(id).Result;
        if (country == null)
            return NotFound();

        return Ok(new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            Movies = country.Movies.Select(movie => new MovieDto
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
            }).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CountryDto countryDto)
    {
        var country = new Country
        {
            Name = countryDto.Name
        };

        try
        {
            await _countryService.AddAsync(country);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CountryDto countryDto)
    {
        var country = await _countryService.GetByIdAsync(id);
        if (country == null)
            return NotFound();

        country.Name = countryDto.Name;

        try
        {
            await _countryService.UpdateAsync(country);
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
        var country = await _countryService.GetByIdAsync(id);
        if (country == null)
            return NotFound();

        try
        {
            await _countryService.DeleteAsync(country);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
}