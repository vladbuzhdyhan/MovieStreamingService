using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class SubscriptionDataGenerator
{
    public static List<Subscription> GenerateSubscriptions(int count)
    {
        return new Faker<Subscription>()
            .RuleFor(s => s.Name, f => f.Lorem.Word())
            .RuleFor(s => s.Description, f => f.Lorem.Sentence())
            .RuleFor(s => s.Price, f => f.Random.Decimal(1, 100))
            .Generate(count);
    }
}