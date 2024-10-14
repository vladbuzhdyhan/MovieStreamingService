using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class EpisodeService : Service<Episode>, IEpisodeService
{
    private readonly IEpisodeRepository _episodeRepository;

    public EpisodeService(IEpisodeRepository episodeRepository) : base(episodeRepository)
    {
        _episodeRepository = episodeRepository;
    }
}