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

    public User Register(User user)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        user.Id = Guid.NewGuid();
        user.Role = Role.User;
        user.Name = user.Login;
        user.LastSeenAt = DateTime.Now;
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;

        _userRepository.AddAsync(user);
        return user;
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}