using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class ReviewDataGenerator
{
    public static List<Review> GenerateReviews(int count, List<Guid> userIds, List<Movie> movies)
    {
        var movieIds = movies.Select(m => m.Id).ToList();

        return new Faker<Review>()
            .RuleFor(r => r.Text, f => f.Lorem.Paragraph())
            .RuleFor(r => r.MovieId, f => f.PickRandom(movieIds))
            .RuleFor(r => r.UserId, f => f.PickRandom(userIds))
            .RuleFor(r => r.Rating, f => f.Random.Int(1, 10))
            .RuleFor(r => r.CreatedAt, f => DateTime.Now)
            .RuleFor(r => r.UpdatedAt, f => DateTime.Now)
            .Generate(count);
    }
}