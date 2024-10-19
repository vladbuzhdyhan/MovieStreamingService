using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class StatusDataGenerator
{
    public static List<Status> GenerateStatuses(int count)
    {
        return new Faker<Status>()
            .RuleFor(s => s.Name, f => f.Lorem.Word())
            .RuleFor(s => s.Slug, f => f.Lorem.Slug())
            .Generate(count);
    }
}