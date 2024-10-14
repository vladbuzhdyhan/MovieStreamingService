using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MovieStreamingService.Domain.Models;
using Type = MovieStreamingService.Domain.Models.Type;

namespace MovieStreamingService.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Country> Country { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<Studio> Studios { get; set; }
    public DbSet<Type> Types { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}