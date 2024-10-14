using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(s => new { s.MovieId, s.GroupId });

        builder.Property(s => s.Number).IsRequired();
        builder.Property(s => s.Name).IsRequired();

        builder.HasOne(s => s.Movie)
            .WithMany(m => m.Seasons)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}