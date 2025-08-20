using Microsoft.Extensions.DependencyInjection;
using Socializer.Chat.Interfaces;
using Socializer.Chat.Services;
using Socializer.Services.Interfaces.Chat;
using Socializer.Services.Services.Chat;

namespace Socializer.Chat.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddScoped<IChatMessageService, ChatMessageService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ICommandsService, CommandsService>();

        return services;
    }
}
