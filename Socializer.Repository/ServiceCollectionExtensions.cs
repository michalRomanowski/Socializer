using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Socializer.Database.NoSql.Extensions;
using Socializer.Repository.Interfaces;
using Socializer.Repository.Repositories;

namespace Socializer.Repository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTableServiceClient(configuration);

        services.AddSingleton<IChatRepository, ChatRepository>();
        services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();

        return services;
    }
}
