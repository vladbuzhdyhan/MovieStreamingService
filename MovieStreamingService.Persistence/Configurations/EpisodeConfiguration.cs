using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Number).IsRequired();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.Image).IsRequired();
        builder.Property(e => e.SeasonId).IsRequired();
        builder.Property(e => e.Duration).IsRequired();
        builder.Property(e => e.AirDate).IsRequired();
        builder.Property(e => e.Slug).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.HasMany(e => e.Comments)
            .WithOne()
            .HasForeignKey(c => c.EpisodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Season)
            .WithMany(m => m.Episodes)
            .HasForeignKey(e => e.SeasonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}