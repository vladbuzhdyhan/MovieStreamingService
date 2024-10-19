using Bogus;
using MovieStreamingService.Domain.Enums;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class UserDataGenerator
{
    public static List<User> GenerateUsers(int count)
    {
        return new Faker<User>()
            .RuleFor(u => u.Login, f => f.Internet.UserName())
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Role, f => Role.User)
            .RuleFor(u => u.Birthday, f => f.Date.Past())
            .RuleFor(u => u.LastSeenAt, f => DateTime.Now)
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
            .RuleFor(u => u.CreatedAt, f => DateTime.Now)
            .RuleFor(u => u.UpdatedAt, f => DateTime.Now)

            .Generate(count);
    }
}