using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Slug).IsRequired();

        builder.HasMany(c => c.Movies)
            .WithMany(m => m.Countries);
    }
}