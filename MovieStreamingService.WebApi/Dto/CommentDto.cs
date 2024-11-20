namespace MovieStreamingService.WebApi.Dto;

public class CommentDto
{
    public long Id { get; set; }
    public string Text { get; set; }
    public string Login { get; set; }
    public CommentDto? Parent { get; set; }
    public List<CommentDto> Replies { get; set; } = [];
}