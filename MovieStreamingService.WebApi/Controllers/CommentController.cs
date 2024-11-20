using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Dto;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByEpisodeIdAsync(int episodeId)
    {
        var comments = await _commentService.GetByEpisodeIdAsync(episodeId);

        return Ok(comments.Select(
            comment => new CommentDto
            {
                Id = comment.Id,
                Text = comment.Text,
                Login = comment.User.Login,
                Parent = comment.Parent is null
                    ? null
                    : new CommentDto
                    {
                        Id = comment.Parent.Id,
                        Text = comment.Parent.Text,
                        Login = comment.Parent.User.Login
                    },
                Replies = comment.Replies.Select(
                    reply => new CommentDto
                    {
                        Id = reply.Id,
                        Text = reply.Text,
                        Login = reply.User.Login
                    }
                ).ToList()
            }
        ));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddAsync(int episodeId, string text, int? parentId)
    {
        var id = User.FindFirst("userId");
        if (id == null)
            return BadRequest("Token is required");

        var comment = new Comment
        {
            Text = text,
            UserId = Guid.Parse(id.Value),
            EpisodeId = episodeId,
            ParentId = parentId
        };

        try
        {
            await _commentService.AddAsync(comment);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }


        return Ok();
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateAsync(long id, string text)
    {
        var comment = await _commentService.GetByIdAsync(id);
        if (comment is null)
            return NotFound();

        var userId = User.FindFirst("userId");
        if (userId == null)
            return BadRequest("Token is required");

        if (comment.UserId != Guid.Parse(userId.Value))
            return BadRequest("You can update only your comments");

        comment.Text = text;

        try
        {
            await _commentService.UpdateAsync(comment);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var comment = await _commentService.GetByIdAsync(id);
        if (comment is null)
            return NotFound();

        var userId = User.FindFirst("userId");
        if (userId == null)
            return BadRequest("Token is required");

        if (comment.UserId != Guid.Parse(userId.Value))
            return BadRequest("You can delete only your comments");

        await _commentService.DeleteAsync(comment);

        return Ok();
    }
}