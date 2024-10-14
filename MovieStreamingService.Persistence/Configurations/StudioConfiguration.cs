using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class StudioConfiguration : IEntityTypeConfiguration<Studio>
{
    public void Configure(EntityTypeBuilder<Studio> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.Logo).IsRequired();
        builder.Property(s => s.Description).IsRequired();
        builder.Property(s => s.Slug).IsRequired();

        builder.HasMany(s => s.Movies)
            .WithMany(m => m.Studios);
    }
}