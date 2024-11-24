namespace MovieStreamingService.WebApi.Dto;

public class EpisodeDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
    public int Duration { get; set; }
    public DateTime AirDate { get; set; }
    public string Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaImage { get; set; }
}