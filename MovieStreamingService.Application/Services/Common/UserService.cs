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

        if(_userRepository.GetByLoginAsync(user.Login).Result != null)
        {
            throw new Exception("User with this login already exists");
        }

        if(_userRepository.GetByEmailAsync(user.Email).Result != null)
        {
            throw new Exception("User with this email already exists");
        }

        _userRepository.AddAsync(user);
        return user;
    }

    public User Login(string login, string password)
    {
        var user = _userRepository.GetByLoginAsync(login).Result;

        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new Exception("Invalid login or password");
        }

        return user;
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}