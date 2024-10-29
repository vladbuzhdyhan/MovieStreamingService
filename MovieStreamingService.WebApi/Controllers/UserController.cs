using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Enums;
using MovieStreamingService.WebApi.Dto;
using System.Security.Claims;

namespace MovieStreamingService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            var user = _userService.GetByIdAsync(id).Result;

            if (user == null)
                return NotFound();

            return Ok(new UserDto(
                user.Id,
                user.Login,
                user.Description,
                user.Name,
                user.Email,
                user.Role.ToString(),
                user.Avatar,
                user.Birthday,
                user.LastSeenAt,
                user.Gender.ToString()
                ));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAllAsync().Result;

            return Ok(users.Select(user => new UserDto(
                user.Id,
                user.Login,
                user.Description,
                user.Name,
                user.Email,
                user.Role.ToString(),
                user.Avatar,
                user.Birthday,
                user.LastSeenAt,
                user.Gender.ToString()
            )));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateUser([FromBody] UserDto userDto)
        {
            var id = User.FindFirst("userId");
            if (id == null)
                return BadRequest("Token is required");

            var user = _userService.GetByLoginAsync(userDto.Login).Result;
            if(user != null && user.Id != Guid.Parse(id.Value))
                return BadRequest("User with this login already exists");

            user = _userService.GetByEmailAsync(userDto.Email).Result;
            if (user != null && user.Id != Guid.Parse(id.Value))
                return BadRequest("User with this email already exists");

            user = _userService.GetByIdAsync(Guid.Parse(id.Value)).Result;
            if (user == null)
                return NotFound();

            try
            {
                user.Login = userDto.Login;
                user.Description = userDto.Description;
                user.Name = userDto.Name;
                user.Email = userDto.Email;
                user.Role = Enum.Parse<Role>(userDto.Role);
                user.Avatar = userDto.Avatar;
                user.Birthday = userDto.Birthday;
                user.LastSeenAt = userDto.LastSeenAt;
                user.Gender = Enum.Parse<Gender>(userDto.Gender);
                _userService.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _userService.GetByIdAsync(id).Result;

            if (user == null)
                return NotFound();

            _userService.DeleteAsync(user);

            return Ok();
        }

        [HttpGet("page-{page}&pageSize-{pageSize}")]
        public IActionResult GetUsers(int page, int pageSize)
        {
            var users = _userService.GetByPageAsync(page, pageSize).Result;

            return Ok(users.Select(user => new UserDto(
                user.Id,
                user.Login,
                user.Description,
                user.Name,
                user.Email,
                user.Role.ToString(),
                user.Avatar,
                user.Birthday,
                user.LastSeenAt,
                user.Gender.ToString()
            )));
        }

        [HttpPut("/change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ChangePassword(string password)
        {
            var id = User.FindFirst("userId");
            if(id == null)
                return BadRequest("Token is required");

            var user = _userService.GetByIdAsync(Guid.Parse(id.Value)).Result;

            if (user == null)
                return NotFound();

            try
            {
                _userService.ChangePassword(user, password);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
