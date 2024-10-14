using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Subscription : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}