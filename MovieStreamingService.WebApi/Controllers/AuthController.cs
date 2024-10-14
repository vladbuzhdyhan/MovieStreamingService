using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Application.Services;
using MovieStreamingService.Application.Services.Common;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Controllers.Models;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly JWTTokenService _jwtTokenService;
    public AuthController(IUserService userService, IUserRepository userRepository, JWTTokenService jwtTokenService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel registerModel)
    {
        var user = new User
        {
            Login = registerModel.Username,
            PasswordHash = registerModel.Password,
            Email = registerModel.Email,
            Birthday = registerModel.Birthday,
            Gender = registerModel.Gender
        };

        _userService.Register(user);

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserCredentials loginModel)
    {
        var user = _userRepository.GetByLoginAsync(loginModel.Username).Result;

        if (user == null || !UserService.VerifyPassword(loginModel.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        var token = _jwtTokenService.Generate(user);

        HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60)
            });

        return Ok();
    }
}