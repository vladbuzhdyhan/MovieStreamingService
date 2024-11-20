using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetByEpisodeIdAsync(int episodeId)
    {
        return await Context.Comments
            .Include(c => c.Replies)
            .Include(c => c.User)
            .Where(c => c.EpisodeId == episodeId)
            .ToListAsync();
    }
}