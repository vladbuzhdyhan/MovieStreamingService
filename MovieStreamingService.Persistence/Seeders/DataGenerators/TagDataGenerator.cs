using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class TagDataGenerator
{
    public static List<Tag> GenerateTags(int count, List<Tag>? parentTags)
    {
        return new Faker<Tag>()
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence())
            .RuleFor(t => t.IsGenre, f => f.Random.Bool())
            .RuleFor(t => t.ParentId, f => parentTags != null && parentTags.Count > 0 ? f.PickRandom(parentTags).Id : null)
            .RuleFor(t => t.Slug, f => f.Lorem.Slug())
            .Generate(count);
    }
}