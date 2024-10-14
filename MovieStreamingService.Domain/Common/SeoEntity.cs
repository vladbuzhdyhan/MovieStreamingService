namespace MovieStreamingService.Domain.Common;

public abstract class SeoEntity<TKey> : BaseEntity<TKey>
{
    public string Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaImage { get; set; }
}