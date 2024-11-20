using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(AppDbContext context) : base(context)
    {
    }

    public new async Task<Person?> GetByIdAsync(params object[] keys)
    {
        return await Context.Persons
            .Include(p => p.Movies)
            .FirstOrDefaultAsync(p => p.Id == (int)keys[0]);
    }
}