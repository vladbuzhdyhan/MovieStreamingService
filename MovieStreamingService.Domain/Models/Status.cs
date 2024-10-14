using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Status : SeoEntity<int>
{
    public string Name { get; set; }
    public List<Movie> Movies { get; set; } = [];
}