using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class StudioDataGenerator
{
    public static List<Studio> GenerateStudios(int count)
    {
        return new Faker<Studio>()
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.Logo, f => f.Image.PicsumUrl())
            .RuleFor(s => s.Description, f => f.Lorem.Paragraph())
            .RuleFor(s => s.Slug, f => f.Lorem.Slug())
            .Generate(count);
    }
}