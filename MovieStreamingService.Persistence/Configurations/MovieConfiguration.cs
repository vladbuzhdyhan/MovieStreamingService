using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.Name).IsRequired();
        builder.Property(f => f.Description).IsRequired()
            .HasColumnType("longtext");
        builder.Property(f => f.RestrictedRating).IsRequired();
        builder.Property(f => f.Poster).IsRequired();
        builder.Property(f => f.ImdbRating).IsRequired();
        builder.Property(f => f.TypeId).IsRequired();
        builder.Property(f => f.StatusId).IsRequired();
        builder.Property(f => f.Background).IsRequired();
        builder.Property(f => f.Slug).IsRequired();
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.UpdatedAt).IsRequired();

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

        builder.HasMany(m => m.Episodes)
            .WithOne(e => e.Movie)
            .HasForeignKey(e => e.MovieId)
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