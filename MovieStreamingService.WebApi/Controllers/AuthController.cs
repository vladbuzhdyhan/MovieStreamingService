using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Application.Services;
using MovieStreamingService.Application.Services.Common;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Controllers.Models;
using MovieStreamingService.WebApi.Dto;

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
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
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
            await _userService.Register(user);
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
            user.LastSeenAt = DateTime.UtcNow;
            _userService.UpdateAsync(user).Wait();
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
    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(".AspNetCore.Application.Id", new CookieOptions
        {
            Path = "/",
            Domain = "localhost", 
            Secure = true,        
            SameSite = SameSiteMode.Lax
        });
        return Ok();
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Me()
    {
        var id = User.FindFirst("userId");
        if (id == null)
            return Unauthorized("Token is required");

        var user = await _userService.GetByIdAsync(Guid.Parse(id.Value));
        user.LastSeenAt = DateTime.Now;
        await _userService.UpdateAsync(user);

        return Ok(new UserDto(
            user.Id,
            user.Login,
            user.Description,
            user.Name,
            user.Email,
            user.Role.ToString(),
            null,
            user.Birthday,
            user.LastSeenAt,
            user.Gender.ToString()
            ));
    }
}