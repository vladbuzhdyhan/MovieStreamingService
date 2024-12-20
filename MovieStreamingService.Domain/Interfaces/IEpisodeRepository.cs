﻿using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface IEpisodeRepository : IRepository<Episode>
{
    Task<IEnumerable<Episode>> GetBySeasonIdAsync(int seasonId);
}