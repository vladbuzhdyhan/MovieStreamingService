using MovieStreamingService.Domain.Enums;

namespace MovieStreamingService.WebApi.Dto;
public class MovieDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string RestrictedRating { get; set; }
    public IFormFile Poster { get; set; }
    public int? Duration { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public DateTime? LastAirDate { get; set; }
    public int? AmountOfEpisodes { get; set; }
    public decimal ImdbRating { get; set; }
    public IFormFile Background { get; set; }
    public IFormFile BigPoster { get; set; }
    public IFormFile ImageTitle { get; set; }
    public List<string> Countries { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public List<PersonDto> People { get; set; } = [];
}
