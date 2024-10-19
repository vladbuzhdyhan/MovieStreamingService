using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class EpisodeDataGenerator
{
    public static List<Episode> GenerateEpisodes(int count, List<Movie> movies)
    {
        var movieIds = movies.Select(m => m.Id).ToList();

        return new Faker<Episode>()
            .RuleFor(e => e.Number, f => f.Random.Number(1, 20))
            .RuleFor(e => e.Name, f => f.Lorem.Sentence())
            .RuleFor(e => e.Description, f => f.Lorem.Paragraph())
            .RuleFor(e => e.Image, f => f.Image.PicsumUrl())
            .RuleFor(e => e.MovieId, f => f.PickRandom(movieIds))
            .RuleFor(e => e.Duration, f => f.Random.Number(40, 50))
            .RuleFor(e => e.AirDate, f => f.Date.Past())
            .RuleFor(e => e.Slug, f => f.Lorem.Slug())
            .RuleFor(e => e.CreatedAt, f => DateTime.Now)
            .RuleFor(e => e.UpdatedAt, f => DateTime.Now)
            .Generate(count);
    }
}