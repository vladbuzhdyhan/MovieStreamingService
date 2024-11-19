using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await Context.Tags.Where(t => t.IsGenre == false)
            .Include(t => t.Parent)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetGenresAsync()
    {
        return await Context.Tags.Where(t => t.IsGenre == true)
            .Include(t => t.Parent)
            .ToListAsync();
    }

    public new async Task<Tag?> GetByIdAsync(params object[] keys)
    {
        return await Context.Tags
            .Include(t => t.Movies)
            .Include(t => t.ChildTags)
            .Include(t => t.Parent)
            .FirstOrDefaultAsync(t => t.Id == (int)keys[0]);
    }

    public new async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await Context.Tags
            .Include(t => t.Parent)
            .ToListAsync();
    }
}