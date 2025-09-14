using Microsoft.Extensions.DependencyInjection;
using Socializer.Chat.Interfaces;
using Socializer.Chat.Services;

namespace Socializer.Chat.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddScoped<ICommandsService, CommandsService>();

        return services;
    }
}
