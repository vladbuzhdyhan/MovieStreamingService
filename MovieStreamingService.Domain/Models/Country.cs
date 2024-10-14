using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Country : SeoEntity<int>
{
    public string Name { get; set; }
    public List<Movie> Movies { get; set; } = [];
}