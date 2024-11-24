using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Description).IsRequired()
            .HasColumnType("longtext");
        builder.Property(m => m.RestrictedRating).IsRequired();
        builder.Property(m => m.Poster).IsRequired();
        builder.Property(m => m.ImdbRating).IsRequired();
        builder.Property(m => m.TypeId).IsRequired();
        builder.Property(m => m.StatusId).IsRequired();
        builder.Property(m => m.Background).IsRequired();
        builder.Property(m => m.Slug).IsRequired();
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.UpdatedAt).IsRequired();
        builder.Property(m => m.BigPoster).IsRequired();
        builder.Property(m => m.ImageTitle).IsRequired();

        builder.HasMany(m => m.Tags)
            .WithMany(t => t.Movies);

        builder.HasMany(m => m.Reviews)
            .WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Countries)
            .WithMany(c => c.Movies);

        builder.HasMany(m => m.Seasons)
            .WithOne(s => s.Movie)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Studios)
            .WithMany(s => s.Movies);

        builder.HasMany(m => m.People)
            .WithMany(p => p.Movies);

        builder.HasMany<Favourite>()
            .WithOne(f => f.Movie)
            .HasForeignKey(f => f.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Type)
            .WithMany(t => t.Movies)
            .HasForeignKey(m => m.TypeId);

        builder.HasOne(m => m.Status)
            .WithMany(s => s.Movies)
            .HasForeignKey(m => m.StatusId);

        builder.HasIndex(m => new { m.Name, m.Description})
            .HasDatabaseName("fulltext_movies_index")
            .IsFullText();
    }
}