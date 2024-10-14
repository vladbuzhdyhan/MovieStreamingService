using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.Description).IsRequired();
        builder.Property(s => s.Price).IsRequired();

        builder.HasMany<UserSubscription>()
            .WithOne(us => us.Subscription)
            .HasForeignKey(us => us.SubscriptionId);
    }
}