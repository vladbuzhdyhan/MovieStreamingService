using Bogus;
using MovieStreamingService.Domain.Enums;
using MovieStreamingService.Domain.Models;
using Type = MovieStreamingService.Domain.Models.Type;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class MovieDataGenerator
{
    public static List<Movie> GenerateMovies(int count, List<Type> types, List<Status> statuses)
    {
        var typeIds = types.Select(t => t.Id).ToList();
        var statusIds = statuses.Select(s => s.Id).ToList();

        return new Faker<Movie>()
            .RuleFor(m => m.Name, f => f.Lorem.Word())
            .RuleFor(m => m.Description, f => f.Lorem.Paragraph())
            .RuleFor(m => m.RestrictedRating, f => f.PickRandom<RestrictedRating>())
            .RuleFor(m => m.Poster, f => f.Image.PicsumUrl())
            .RuleFor(m => m.Background, f => f.Image.PicsumUrl())
            .RuleFor(m => m.ImdbRating, f => f.Random.Decimal(1, 10))
            .RuleFor(m => m.TypeId, f => f.PickRandom(typeIds))
            .RuleFor(m => m.StatusId, f => f.PickRandom(statusIds))
            .RuleFor(m => m.Slug, f => f.Lorem.Slug())
            .RuleFor(m => m.CreatedAt, f => DateTime.Now)
            .RuleFor(m => m.UpdatedAt, f => DateTime.Now)
            .Generate(count);
    }
}