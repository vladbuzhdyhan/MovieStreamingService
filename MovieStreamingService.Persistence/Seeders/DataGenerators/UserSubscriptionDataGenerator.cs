using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class UserSubscriptionDataGenerator
{
    public static List<UserSubscription> GenerateUserSubscriptions(int count, List<Guid> userIds, List<Subscription> subscriptions)
    {
        var subscriptionIds = subscriptions.Select(s => s.Id).ToList();

        return new Faker<UserSubscription>()
            .RuleFor(us => us.UserId, f => f.PickRandom(userIds))
            .RuleFor(us => us.SubscriptionId, f => f.PickRandom(subscriptionIds))
            .RuleFor(us => us.StartDate, f => f.Date.Past())
            .RuleFor(us => us.EndDate, f => f.Date.Future())
            .Generate(count);
    }
}