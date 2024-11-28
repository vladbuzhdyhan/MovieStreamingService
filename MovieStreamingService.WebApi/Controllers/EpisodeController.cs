using Bogus;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EpisodeController : ControllerBase
{
    private readonly IEpisodeService _episodeService;

    public static FormFile GetImageFormFile(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return null;

        var memoryStream = new MemoryStream();
        try
        {
            using var stream =
                new FileStream(Path.Combine("wwwroot/episode", imagePath), FileMode.Open, FileAccess.Read);
            stream.CopyTo(memoryStream);
        }
        catch
        {
            return null;
        }

        return new FormFile(memoryStream, 0, memoryStream.Length, "episode", imagePath)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/" + Path.GetExtension(imagePath).ToLower().Remove(0, 1),
            ContentDisposition = $"form-data; name=episode; filename={imagePath}"
        };
    }

    public static FileStreamResult GetVideoStream(string? videoPath)
    {
        if (string.IsNullOrEmpty(videoPath) || !System.IO.File.Exists(Path.Combine("wwwroot/video", videoPath)))
            return null; 

        var fileStream = new FileStream(Path.Combine("wwwroot/video", videoPath), FileMode.Open, FileAccess.Read, FileShare.Read);

        return new FileStreamResult(fileStream, "video/" + Path.GetExtension(videoPath).ToLower().Remove(0, 1))
        {
            EnableRangeProcessing = true
        };
    }

    public EpisodeController(IEpisodeService episodeService)
    {
        _episodeService = episodeService;
    }

    [HttpGet("{seasonId}")]
    public async Task<IActionResult> GetBySeasonId(int seasonId)
    {
        var episodes = await _episodeService.GetBySeasonIdAsync(seasonId);
        return Ok(episodes.Select(
            episode => new EpisodeDto
            {
                Id = episode.Id,
                Number = episode.Number,
                Name = episode.Name,
                Description = episode.Description,
                Duration = episode.Duration,
                AirDate = episode.AirDate,
                Image = GetImageFormFile(episode.Image),
                Slug = episode.Slug,
                MetaTitle = episode.MetaTitle,
                MetaDescription = episode.MetaDescription,
                MetaImage = episode.MetaImage
            }
            ));
    }

    [HttpGet("stream/{id}")]
    public IActionResult StreamVideo(int id)
    {
        var episode = _episodeService.GetByIdAsync(id).Result;
        if (episode == null)
            return NotFound();

        var video = GetVideoStream(episode.Video);
        if (video == null)
            return NotFound();

        return video;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(int seasonId, [FromForm] EpisodeDto episodeDto, IFormFile video)
    {
        if (episodeDto.Image != null)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(episodeDto.Image.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");

            if (episodeDto.Image.Length > 2 * 1024 * 1024)
                return BadRequest("File is too big");
        }

        if (video != null)
        {
            var validExtensions = new[] { ".mp4", ".mov", ".m3u8", ".avi" };
            var fileExtension = Path.GetExtension(video.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");
        }

        var episode = new Episode
        {
            SeasonId = seasonId,
            Number = episodeDto.Number,
            Name = episodeDto.Name,
            Description = episodeDto.Description,
            Duration = episodeDto.Duration,
            AirDate = episodeDto.AirDate,
            Slug = episodeDto.Slug,
            MetaTitle = episodeDto.MetaTitle,
            MetaDescription = episodeDto.MetaDescription,
            MetaImage = episodeDto.MetaImage
        };

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(episodeDto.Image.FileName).ToLower()}";
        var filePath = Path.Combine("wwwroot/episode", fileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await episodeDto.Image.CopyToAsync(fileStream);
        }

        episode.Image = fileName;

        var videoName = $"{Guid.NewGuid()}{Path.GetExtension(episodeDto.Image.FileName).ToLower()}";
        var videoPath = Path.Combine("wwwroot/video", videoName);
        await using (var fileStream = new FileStream(videoPath, FileMode.Create))
        {
            await video.CopyToAsync(fileStream);
        }

        episode.Video = videoName;

        try
        {
            await _episodeService.AddAsync(episode);
        } catch
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromForm] EpisodeDto episodeDto, IFormFile? video)
    {
        var episode = await _episodeService.GetByIdAsync(id);
        if (episode == null)
            return NotFound();

        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(episodeDto.Image.FileName).ToLower();
        if (!validExtensions.Contains(fileExtension))
            return BadRequest("This file type is not supported");

        if (episodeDto.Image.Length > 2 * 1024 * 1024)
            return BadRequest("File is too big");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(episodeDto.Image.FileName).ToLower()}";
        var filePath = Path.Combine("wwwroot/episode", fileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await episodeDto.Image.CopyToAsync(fileStream);
        }

        System.IO.File.Delete(Path.Combine("wwwroot/episode", episode.Image));
        episode.Image = fileName;

        if (video != null) {
            validExtensions = [".mp4", ".mov", ".m3u8", ".avi"];
            fileExtension = Path.GetExtension(video.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("This file type is not supported");

            fileName = $"{Guid.NewGuid()}{Path.GetExtension(video.FileName).ToLower()}";
            filePath = Path.Combine("wwwroot/video", fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await video.CopyToAsync(fileStream);
            }

            System.IO.File.Delete(Path.Combine("wwwroot/video", episode.Video));
            episode.Video = fileName;
        }

        episode.Number = episodeDto.Number;
        episode.Name = episodeDto.Name;
        episode.Description = episodeDto.Description;
        episode.Duration = episodeDto.Duration;
        episode.AirDate = episodeDto.AirDate;
        episode.Slug = episodeDto.Slug;
        episode.MetaTitle = episodeDto.MetaTitle;
        episode.MetaDescription = episodeDto.MetaDescription;
        episode.MetaImage = episodeDto.MetaImage;

        try
        {
            await _episodeService.UpdateAsync(episode);
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var episode = await _episodeService.GetByIdAsync(id);
        if (episode == null)
            return NotFound();

        System.IO.File.Delete(Path.Combine("wwwroot/episode", episode.Image));
        System.IO.File.Delete(Path.Combine("wwwroot/video", episode.Video));

        await _episodeService.DeleteAsync(episode);

        return Ok();
    }
}