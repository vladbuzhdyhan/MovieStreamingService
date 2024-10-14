namespace MovieStreamingService.Domain.Common;

public class AuditableSeoEntity<TKey> : SeoEntity<TKey>
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}