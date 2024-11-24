using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Season : BaseEntity<int>
{
    public int MovieId { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public Movie Movie { get; set; }
    public List<Episode> Episodes { get; set; } = [];
}