using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Enums;
using MovieStreamingService.WebApi.Dto;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IFavouriteService _favouriteService;
        private readonly IUserService _userService;
        private static FormFile? GetAvatarFormFile(string? avatarPath)
        {
            if (string.IsNullOrEmpty(avatarPath))
                return null;

            var memoryStream = new MemoryStream();
            try
            {
                using var stream = new FileStream(Path.Combine("wwwroot/avatars", avatarPath), FileMode.Open, FileAccess.Read);
                stream.CopyTo(memoryStream);
            } catch {
                return null;
            }

            return new FormFile(memoryStream, 0, memoryStream.Length, "avatar", avatarPath)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/"+Path.GetExtension(avatarPath).ToLower().Remove(0, 1),
                ContentDisposition = $"form-data; name=avatar; filename={avatarPath}"
            };
        }

        public UserController(IFavouriteService favouriteService, IUserService userService)
        {
            _favouriteService = favouriteService;
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
                GetAvatarFormFile(user.Avatar),
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
                GetAvatarFormFile(user.Avatar),
                user.Birthday,
                user.LastSeenAt,
                user.Gender.ToString()
            )));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
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

            if (userDto.Avatar != null)
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(userDto.Avatar.FileName).ToLower();
                if (!validExtensions.Contains(fileExtension))
                    return BadRequest("This file type is not supported");

                if (userDto.Avatar.Length > 2 * 1024 * 1024)
                    return BadRequest("File is too big");
            }

            try
            {
                user.Login = userDto.Login;
                user.Description = userDto.Description;
                user.Name = userDto.Name;
                user.Email = userDto.Email;
                user.Role = Enum.Parse<Role>(userDto.Role);
                user.Birthday = userDto.Birthday;
                user.LastSeenAt = DateTime.Now;
                user.Gender = Enum.Parse<Gender>(userDto.Gender);

                if(userDto.Avatar != null)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(userDto.Avatar.FileName).ToLower()}";
                    var filePath = Path.Combine("wwwroot/avatars", fileName);
                    await using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await userDto.Avatar.CopyToAsync(fileStream);
                    }
                    user.Avatar = fileName;
                } else if(user.Avatar != null && userDto.Avatar == null)
                {
                    System.IO.File.Delete(Path.Combine("wwwroot/avatars", user.Avatar));
                    user.Avatar = null;
                }

                await _userService.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = _userService.GetByIdAsync(id).Result;

            if (user == null)
                return NotFound();

            await _userService.DeleteAsync(user);

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
                GetAvatarFormFile(user.Avatar),
                user.Birthday,
                user.LastSeenAt,
                user.Gender.ToString()
            )));
        }

        [HttpPut("/change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(string password)
        {
            var id = User.FindFirst("userId");
            if(id == null)
                return BadRequest("Token is required");

            var user = _userService.GetByIdAsync(Guid.Parse(id.Value)).Result;

            if (user == null)
                return NotFound();

            try
            {
                await _userService.ChangePassword(user, password);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut("/change-role")]
        public async Task<IActionResult> ChangeRole(Guid id, string role)
        {
            var user = _userService.GetByIdAsync(id).Result;

            if (user == null)
                return NotFound();

            try
            {
                user.Role = Enum.Parse<Role>(role);
                await _userService.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet("/favourite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetFavourites()
        {
            var id = User.FindFirst("userId");
            if (id == null)
                return BadRequest("Token is required");

            var favourites = _favouriteService.GetByUserIdAsync(Guid.Parse(id.Value)).Result;

            return Ok(favourites.Select(f => f.Movie));
        }

        [HttpPost("/favourite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddFavourite(int movieId)
        {
            var id = User.FindFirst("userId");
            if (id == null)
                return BadRequest("Token is required");

            try
            {
                await _favouriteService.AddAsync(new Favourite
                {
                    MovieId = movieId,
                    UserId = Guid.Parse(id.Value),
                    AddDate = DateTime.Now
                });
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("/favourite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteFavourite(int movieId)
        {
            var id = User.FindFirst("userId");
            if (id == null)
                return BadRequest("Token is required");

            var favourite = _favouriteService.GetByIdAsync(Guid.Parse(id.Value), movieId).Result;

            if (favourite == null)
                return NotFound();

            await _favouriteService.DeleteAsync(favourite);

            return Ok();
        }

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfile()
        {
            var id = User.FindFirst("userId");
            if (id == null)
                return Unauthorized("Token is required");

            var user = await _userService.GetByIdAsync(Guid.Parse(id.Value));
            if (user == null)
                return NotFound();

            return Ok(new UserDto
            (
                user.Id,
                user.Login,
                user.Description,
                user.Name,
                user.Email,
                user.Role.ToString(),
                GetAvatarFormFile(user.Avatar),
                user.Birthday,
                user.LastSeenAt,
                user.Gender.ToString()
            ));
        }

        [HttpPost("upload-avatar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadAvatar(IFormFile? avatar)
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("Не вдалося завантажити файл аватара");

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(avatar.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
                return BadRequest("Непідтримуваний формат файлу");

            if (avatar.Length > 2 * 1024 * 1024)
                return BadRequest("Розмір файлу перевищує допустимий ліміт");

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine("wwwroot/avatars", fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                var userId = User.FindFirst("userId")?.Value;
                if (userId == null)
                    return BadRequest("Користувача не знайдено");

                var user = await _userService.GetByIdAsync(Guid.Parse(userId));
                if (user == null)
                    return NotFound("Користувач не знайдений");

                user.Avatar = fileName;
                await _userService.UpdateAsync(user);

                return Ok(new { message = "Аватар успішно завантажено", path = user.Avatar });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка при збереженні аватара: {ex.Message}");
            }
        }
    }
}
