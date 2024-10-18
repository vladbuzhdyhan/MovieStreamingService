using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLoginAsync(string login);

    Task<User?> GetByEmailAsync(string email);
}