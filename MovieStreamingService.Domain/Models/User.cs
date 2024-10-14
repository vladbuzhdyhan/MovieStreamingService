using MovieStreamingService.Domain.Common;
using MovieStreamingService.Domain.Enums;

namespace MovieStreamingService.Domain.Models;

public class User : AuditableEntity<Guid>
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string? Avatar { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime LastSeenAt { get; set; }
    public Gender Gender { get; set; }
    public List<Favourite> Favourites { get; set; } = [];
}