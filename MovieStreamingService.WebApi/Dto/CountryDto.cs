namespace MovieStreamingService.WebApi.Dto;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MovieDto> Movies { get; set; } = [];
}