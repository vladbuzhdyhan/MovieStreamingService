using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class StudioService : Service<Studio>, IStudioService
{
    private readonly IStudioRepository _studioRepository;
    public StudioService(IStudioRepository studioRepository) : base(studioRepository)
    {
        _studioRepository = studioRepository;
    }
}