using Microsoft.Extensions.DependencyInjection;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Application.Services;
using MovieStreamingService.Application.Services.Common;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MovieStreamingService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["jwt-key"];
        services.AddSingleton(secretKey);

        services.AddServices();

        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IFavouriteService, FavouriteService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<DataSeederService>();
        services.AddScoped<JWTTokenService>();
    }
}