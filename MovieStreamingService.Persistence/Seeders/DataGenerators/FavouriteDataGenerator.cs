using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class FavouriteDataGenerator
{
    public static List<Favourite> GenerateFavourites(int count, List<Guid> userIds, List<Movie> movies)
    {
        var movieIds = movies.Select(m => m.Id).ToList();

        return new Faker<Favourite>()
            .RuleFor(f => f.UserId, f => f.PickRandom(userIds))
            .RuleFor(f => f.MovieId, f => f.PickRandom(movieIds))
            .RuleFor(f => f.AddDate, f => DateTime.Now)
            .Generate(count);
    }
}