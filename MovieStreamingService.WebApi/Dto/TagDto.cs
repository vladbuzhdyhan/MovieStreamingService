namespace MovieStreamingService.WebApi.Dto;

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsGenre { get; set; }
    public TagDto? Parent { get; set; }
    public List<MovieDto> Movies { get; set; } = [];
    public List<TagDto> ChildTags { get; set; } = [];
}