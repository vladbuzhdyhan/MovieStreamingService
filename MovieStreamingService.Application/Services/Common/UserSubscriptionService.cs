using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class UserSubscriptionService : Service<UserSubscription>, IUserSubscriptionService
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;

    public UserSubscriptionService(IUserSubscriptionRepository userSubscriptionRepository) : base(userSubscriptionRepository)
    {
        _userSubscriptionRepository = userSubscriptionRepository;
    }
}