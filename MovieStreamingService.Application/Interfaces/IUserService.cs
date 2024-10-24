using MovieStreamingService.Domain.Enums;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface IUserService : IService<User>
{
    Task<User?> GetByLoginAsync(string login);
    Task<User?> GetByEmailAsync(string email);
    Task Register(User user);
    User Login(string login, string password);
}