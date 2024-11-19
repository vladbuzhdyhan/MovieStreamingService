using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.Description).IsRequired();
        builder.Property(t => t.IsGenre).IsRequired();
        builder.Property(t => t.Slug).IsRequired();

        builder.HasMany(t => t.Movies)
            .WithMany(m => m.Tags);

        builder.HasMany(t => t.ChildTags)
            .WithOne(t => t.Parent)
            .HasForeignKey(t => t.ParentId);
    }
}