using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var tags = _tagService.GetAllAsync().Result;
        return Ok(tags.Select(
            tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                IsGenre = tag.IsGenre,
                Parent = tag.Parent is null
                    ? null
                    : new TagDto
                    {
                        Id = tag.Parent.Id,
                        Name = tag.Parent.Name,
                        Description = tag.Parent.Description,
                        IsGenre = tag.Parent.IsGenre
                    },
            }
        ));
    }

    [HttpGet("page-{page}&pageSize-{pageSize}")]
    public IActionResult GetPage(int page, int pageSize)
    {
        var tags = _tagService.GetByPageAsync(page, pageSize).Result;
        return Ok(tags.Select(
            tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                IsGenre = tag.IsGenre,
                Parent = tag.Parent is null
                    ? null
                    : new TagDto
                    {
                        Id = tag.Parent.Id,
                        Name = tag.Parent.Name,
                        Description = tag.Parent.Description,
                        IsGenre = tag.Parent.IsGenre
                    },
            }
        ));
    }

    [HttpGet("genres")]
    public IActionResult GetGenres()
    {
        var genres = _tagService.GetGenresAsync().Result;
        return Ok(genres.Select(
            genre => new TagDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description,
                IsGenre = genre.IsGenre,
                Parent = genre.Parent is null
                    ? null
                    : new TagDto
                    {
                        Id = genre.Parent.Id,
                        Name = genre.Parent.Name,
                        Description = genre.Parent.Description,
                        IsGenre = genre.Parent.IsGenre
                    },
            }
        ));
    }

    [HttpGet("tags")]
    public IActionResult GetTags()
    {
        var tags = _tagService.GetTagsAsync().Result;
        return Ok(tags.Select(
            tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                IsGenre = tag.IsGenre,
                Parent = tag.Parent is null
                    ? null
                    : new TagDto
                    {
                        Id = tag.Parent.Id,
                        Name = tag.Parent.Name,
                        Description = tag.Parent.Description,
                        IsGenre = tag.Parent.IsGenre
                    },
            }
        ));
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var tag = _tagService.GetByIdAsync(id).Result;
        if (tag is null)
        {
            return NotFound();
        }

        return Ok(new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            IsGenre = tag.IsGenre,
            Parent = tag.Parent is null
                ? null
                : new TagDto
                {
                    Id = tag.Parent.Id,
                    Name = tag.Parent.Name,
                    Description = tag.Parent.Description,
                    IsGenre = tag.Parent.IsGenre
                },
            Movies = tag.Movies.Select(
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
                ).ToList(),
            ChildTags = tag.ChildTags.Select(
                childTag => new TagDto
                {
                    Id = childTag.Id,
                    Name = childTag.Name,
                    Description = childTag.Description,
                    IsGenre = childTag.IsGenre,
                }
                ).ToList()
        });
    }

    [HttpPost]
    public IActionResult Post([FromBody] TagDto tagDto)
    {
        var tag = new Tag
        {
            Name = tagDto.Name,
            Description = tagDto.Description,
            IsGenre = tagDto.IsGenre,
            ParentId = tagDto.Parent?.Id
        };

        try
        {
            _tagService.AddAsync(tag);
        } catch(Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] TagDto tagDto)
    {
        var tag = _tagService.GetByIdAsync(id).Result;
        if (tag is null)
            return NotFound();

        tag.Name = tagDto.Name;
        tag.Description = tagDto.Description;
        tag.IsGenre = tagDto.IsGenre;
        tag.ParentId = tagDto.Parent?.Id;

        try
        {
            _tagService.UpdateAsync(tag);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = _tagService.GetByIdAsync(id).Result;
        if (tag is null)
            return NotFound();

        await _tagService.DeleteAsync(tag);

        return Ok();
    }
}