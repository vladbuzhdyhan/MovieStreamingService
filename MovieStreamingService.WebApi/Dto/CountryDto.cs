namespace MovieStreamingService.WebApi.Dto;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieDto> Movies { get; set; } = [];
    public string Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaImage { get; set; }
}