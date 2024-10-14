using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Type = MovieStreamingService.Domain.Models.Type;

namespace MovieStreamingService.Persistence.Configurations;

public class TypeConfiguration : IEntityTypeConfiguration<Type>
{
    public void Configure(EntityTypeBuilder<Type> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.Description).IsRequired();
        builder.Property(t => t.Slug).IsRequired();

        builder.HasMany(t => t.Movies)
            .WithOne(m => m.Type)
            .HasForeignKey(m => m.TypeId);
    }
}