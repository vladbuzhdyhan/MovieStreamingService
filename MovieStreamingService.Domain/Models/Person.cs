using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Person : AuditableSeoEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Image { get; set; }
    public string? Biography { get; set; }
    public List<Movie> Movies { get; set; } = [];
}