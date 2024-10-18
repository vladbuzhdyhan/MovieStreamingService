using MovieStreamingService.Domain.Enums;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Interfaces;

public interface IUserService : IService<User>
{
    User Register(User user);
    User Login(string login, string password);
}