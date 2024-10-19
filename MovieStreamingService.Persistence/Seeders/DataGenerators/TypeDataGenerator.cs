using Bogus;
using Type = MovieStreamingService.Domain.Models.Type;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class TypeDataGenerator
{
    public static List<Type> GenerateTypes(int count)
    {
        return new Faker<Type>()
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence())
            .RuleFor(t => t.Slug, f => f.Lorem.Slug())
            .Generate(count);
    }
}