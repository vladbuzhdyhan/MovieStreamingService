using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStreamingService.Persistence.Context;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Persistence.Repositories;
using MovieStreamingService.Persistence.Seeders;

namespace MovieStreamingService.Persistence;

public static class DependencyInjection
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MovieStreamingServiceDbContext");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 0)));
        });
        services.InitRepositories();
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        services.AddScoped<IFavouriteRepository, FavouriteRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ISeasonRepository, SeasonRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IStudioRepository, StudioRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();
        services.AddScoped<DataSeederHelper>();
    }
}