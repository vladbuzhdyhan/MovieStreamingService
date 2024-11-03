﻿using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Domain.Interfaces;

public interface IMovieRepository : IRepository<Movie>
{
    Task<IEnumerable<Movie>> SearchAsync(string search);
}