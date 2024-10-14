namespace MovieStreamingService.Domain.Models;

public class Favourite
{
    public Guid UserId { get; set; }
    public int MovieId { get; set; }
    public DateTime AddDate { get; set; }
    public Movie Movie { get; set; }
}