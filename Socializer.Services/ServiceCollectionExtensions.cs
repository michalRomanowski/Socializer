using Microsoft.Extensions.DependencyInjection;
using Socializer.Services.Interfaces;
using Socializer.Services.Interfaces.Chat;
using Socializer.Services.Services;
using Socializer.Services.Services.Chat;

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
        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}
