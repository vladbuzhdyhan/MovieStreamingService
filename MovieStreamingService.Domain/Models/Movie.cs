using MovieStreamingService.Domain.Common;
using MovieStreamingService.Domain.Enums;

namespace MovieStreamingService.Domain.Models;

public class Movie : AuditableSeoEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public RestrictedRating RestrictedRating { get; set; }
    public string Poster { get; set; }
    public int? Duration { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public DateTime? LastAirDate { get; set; }
    public int? AmountOfEpisodes { get; set; }
    public decimal ImdbRating { get; set; }
    public int TypeId { get; set; }
    public int StatusId { get; set; }
    public string Background { get; set; }
    public string BigPoster { get; set; }
    public string ImageTitle { get; set; }
    public List<Tag> Tags { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Country> Countries { get; set; } = [];
    public List<Season> Seasons { get; set; } = [];
    public List<Episode> Episodes { get; set; } = [];
    public List<Studio> Studios { get; set; } = [];
    public List<Person> People { get; set; } = [];
    public Status Status { get; set; }
    public Type Type { get; set; }
}