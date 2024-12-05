using Microsoft.Extensions.DependencyInjection;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Application.Services;
using MovieStreamingService.Application.Services.Common;
using Microsoft.Extensions.Configuration;

namespace MovieStreamingService.Application;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(sp =>
        {
            var secretKey = configuration["jwt-key"];
            return new JWTTokenService(secretKey);
        });
        services.AddScoped(sp =>
        {
            var liqPublicKey = configuration["liq-public-key"];
            var liqPrivateKey = configuration["liq-private-key"];
            return new PaymentService(liqPublicKey, liqPrivateKey);
        });

        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IEpisodeService, EpisodeService>();
        services.AddScoped<IFavouriteService, FavouriteService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSubscriptionService, UserSubscriptionService>();
        services.AddScoped<DataSeederService>();
    }
}