using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.HasKey(f => new { f.UserId, f.MovieId });

        builder.Property(f => f.AddDate).IsRequired();

        builder.HasOne<User>()
            .WithMany(u => u.Favourites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Movie)
            .WithMany()
            .HasForeignKey(f => f.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}