namespace MovieStreamingService.Domain.Common;

public class AuditableEntity<TKey> : BaseEntity<TKey>
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}