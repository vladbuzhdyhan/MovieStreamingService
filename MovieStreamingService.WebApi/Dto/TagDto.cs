namespace MovieStreamingService.WebApi.Dto;

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsGenre { get; set; }
    public string Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaImage { get; set; }
    public TagDto? Parent { get; set; }
    public List<MovieDto> Movies { get; set; } = [];
    public List<TagDto> ChildTags { get; set; } = [];
}