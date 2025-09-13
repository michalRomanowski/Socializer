using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Socializer.Database.NoSql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTableServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddTableServiceClient(configuration["ChatAzureStorageConnectionString:tableServiceUri"]);
        });

        return services;
    }
}
