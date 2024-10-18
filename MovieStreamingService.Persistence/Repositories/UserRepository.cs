using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.Persistence.Context;

namespace MovieStreamingService.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}