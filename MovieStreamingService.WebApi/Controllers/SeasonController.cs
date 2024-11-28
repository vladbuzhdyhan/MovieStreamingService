using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeasonController : ControllerBase
{
    private readonly ISeasonService _seasonService;

    public SeasonController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    [HttpGet("{movieId}")]
    public async Task<IActionResult> GetByMovieId(int movieId)
    {
        var seasons = await _seasonService.GetByMovieIdAsync(movieId);
        return Ok(seasons.Select(
            season => new SeasonDto
            {
                Id = season.Id,
                Number = season.Number,
                Name = season.Name
            }
            ));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int movieId, [FromBody] SeasonDto seasonDto)
    {
        var season = new Season
        {
            MovieId = movieId,
            Number = seasonDto.Number,
            Name = seasonDto.Name
        };

        try
        {
            await _seasonService.AddAsync(season);
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SeasonDto seasonDto)
    {
        var season = new Season
        {
            Id = id,
            Number = seasonDto.Number,
            Name = seasonDto.Name
        };

        try
        {
            await _seasonService.UpdateAsync(season);
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var season = await _seasonService.GetByIdAsync(id);
        if (season == null)
            return NotFound();

        await _seasonService.DeleteAsync(season);

        return Ok();
    }
}