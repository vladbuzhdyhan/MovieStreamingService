using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Comment : AuditableEntity<long>
{
    public string Text { get; set; }
    public int EpisodeId { get; set; }
    public long? ParentId { get; set; }
    public Guid UserId { get; set; }
    public List<Comment> Replies { get; set; } = [];
    public User User { get; set; }
}