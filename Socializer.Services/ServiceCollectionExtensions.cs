using Microsoft.Extensions.DependencyInjection;
using Socializer.Services.Interfaces;
using Socializer.Services.Services;

namespace Socializer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPreferenceService, PreferenceService>();
        services.AddScoped<IExtractPreferencesService, ExtractPreferencesService>();
        services.AddScoped<IReadPreferencesService, ReadPreferencesService>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();
        services.AddScoped<IUserMatchingService, UserMatchingService>();

        return services;
    }
}
