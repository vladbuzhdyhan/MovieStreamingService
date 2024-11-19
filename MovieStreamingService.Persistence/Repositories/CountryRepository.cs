using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class CountryRepository : Repository<Country>, ICountryRepository
{
    public CountryRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<Country?> GetByIdAsync(params object[] keys)
    {
        return await Context.Country
            .Include(c => c.Movies)
            .FirstOrDefaultAsync(c => c.Id == (int)keys[0]);
    }
}