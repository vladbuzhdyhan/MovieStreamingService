using MovieStreamingService.Domain.Enums;

namespace MovieStreamingService.WebApi.Dto;

public record UserDto(Guid Id, 
    string Login,
    string? Description,
    string Name,
    string Email,
    string Role,
    IFormFile? Avatar,
    DateTime Birthday,
    DateTime LastSeenAt,
    string Gender
    );