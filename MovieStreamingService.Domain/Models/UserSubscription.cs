namespace MovieStreamingService.Domain.Models;

public class UserSubscription
{
    public Guid UserId { get; set; }
    public int SubscriptionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Subscription Subscription { get; set; }
}