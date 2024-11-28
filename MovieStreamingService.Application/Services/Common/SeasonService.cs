using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class SeasonService : Service<Season>, ISeasonService
{
    private readonly ISeasonRepository _seasonRepository;
    public SeasonService(ISeasonRepository seasonRepository) : base(seasonRepository)
    {
        _seasonRepository = seasonRepository;
    }

    public async Task<IEnumerable<Season>> GetByMovieIdAsync(int movieId)
    {
        return await _seasonRepository.GetByMovieIdAsync(movieId);
    }
}