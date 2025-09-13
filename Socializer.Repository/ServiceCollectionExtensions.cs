using Microsoft.Extensions.DependencyInjection;
using Socializer.Repository.Interfaces;
using Socializer.Repository.Repositories;

namespace Socializer.Repository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();

        return services;
    }
}
