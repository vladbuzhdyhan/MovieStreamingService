using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class CountryDataGenerator
{
    public static List<Country> GenerateCountries(int count)
    {
        return new Faker<Country>()
            .RuleFor(c => c.Name, f => f.Address.Country())
            .RuleFor(c => c.Slug, f => f.Lorem.Slug())
            .Generate(count);
    }
}