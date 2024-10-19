using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class PersonDataGenerator
{
    public static List<Person> GeneratePeople(int count)
    {
        return new Bogus.Faker<Person>()
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Biography, f => f.Lorem.Paragraph())
            .RuleFor(p => p.BirthDate, f => f.Date.Past())
            .RuleFor(p => p.Slug, f => f.Lorem.Slug())
            .RuleFor(p => p.CreatedAt, f => DateTime.Now)
            .RuleFor(p => p.UpdatedAt, f => DateTime.Now)
            .Generate(count);
    }
}