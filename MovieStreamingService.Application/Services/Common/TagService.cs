using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class TagService : Service<Tag>, ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository) : base(tagRepository)
    {
        _tagRepository = tagRepository;
    }
}