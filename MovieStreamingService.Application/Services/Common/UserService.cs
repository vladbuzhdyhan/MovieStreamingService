using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Enums;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class UserService : Service<User>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) : base(userRepository)
    {
        _userRepository = userRepository;
    }

    public User Register(string login, string plainPassword)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        return new User { 
            Login = login, 
            PasswordHash = hashedPassword, 
            Role = Role.User 
        };
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}