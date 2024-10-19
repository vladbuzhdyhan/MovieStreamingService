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
    private readonly JWTTokenService _jwtTokenService;
    public AuthController(IUserService userService, JWTTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel registerModel)
    {
        var user = new User
        {
            Login = registerModel.Login,
            PasswordHash = registerModel.Password,
            Email = registerModel.Email,
            Birthday = registerModel.Birthday,
            Gender = registerModel.Gender
        };

        try {
            _userService.Register(user);
        } catch (Exception e) {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserCredentials loginModel)
    {
        User user;
        try {
            user = _userService.Login(loginModel.Login, loginModel.Password);
        } catch (Exception e) {
            return BadRequest(e.Message);
        }

        var token = _jwtTokenService.Generate(user);

        HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(180)
            });

        return Ok();
    }
}