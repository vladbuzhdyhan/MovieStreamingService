using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Tag : SeoEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsGenre { get; set; }
    public int? ParentId { get; set; }
    public Tag? Parent { get; set; }
    public List<Movie> Movies { get; set; } = [];
    public List<Tag> ChildTags { get; set; } = [];
}