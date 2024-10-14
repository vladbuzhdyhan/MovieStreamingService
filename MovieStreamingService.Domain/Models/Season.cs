namespace MovieStreamingService.Domain.Models;

public class Season
{
    public int MovieId { get; set; }
    public int GroupId { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public Movie Movie { get; set; }
}