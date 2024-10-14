using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Review : AuditableEntity<int>
{
    public string? Text { get; set; }
    public int Rating { get; set; }
    public int MovieId { get; set; }
    public Guid UserId { get; set; }
    public Movie Movie { get; set; }
    public User User { get; set; }
}