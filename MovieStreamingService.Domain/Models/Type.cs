using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Type : SeoEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Movie> Movies { get; set; } = [];
}