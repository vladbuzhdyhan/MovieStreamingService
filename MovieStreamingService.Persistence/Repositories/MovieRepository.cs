using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Movie>> SearchAsync(string search)
    {
        return await Context.Movies
            .FromSqlRaw("SELECT * FROM Movies WHERE MATCH(name, Description) AGAINST({0})", search)
            .Include(m => m.Type)
            .Include(m => m.Status)
            .Include(m => m.Countries)
            .Include(m => m.Tags)
            .ToListAsync();
    }

    public new async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await Context.Movies
            .Include(m => m.Type)
            .Include(m => m.Status)
            .Include(m => m.Countries)
            .Include(m => m.Tags)
            .ToListAsync();
    }

    public new async Task<Movie?> GetByIdAsync(params object[] keys)
    {
        return await Context.Movies
            .Include(m => m.Type)
            .Include(m => m.Status)
            .Include(m => m.Countries)
            .Include(m => m.Tags)
            .FirstAsync(m => m.Id == (int)keys[0]);
    }
}