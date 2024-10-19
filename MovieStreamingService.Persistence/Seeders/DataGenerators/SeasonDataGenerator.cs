using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class SeasonDataGenerator
{
    public static List<Season> GenerateSeasons(int count, List<Movie> movies)
    {
        var moviesId = movies.Select(m => m.Id).ToList();

        return new Faker<Season>()
            .RuleFor(s => s.Number, f => f.Random.Number(1, 10))
            .RuleFor(s => s.MovieId, f => f.PickRandom(moviesId))
            .RuleFor(s => s.Name, f => f.Lorem.Sentence())
            .RuleFor(s => s.GroupId, f => f.Random.Number(1, 10))
            .Generate(count);
    }
}