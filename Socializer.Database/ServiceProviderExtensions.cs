using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Socializer.Database;

public static class ServiceProviderExtensions
{
    public static IServiceProvider MigrateSocializerDatabase(this IServiceProvider serviceProvider, ILogger logger)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var socializerDbContext = scope.ServiceProvider.GetRequiredService<SocializerDbContext>();
            socializerDbContext.Database.Migrate();
            logger.LogInformation("Socializer Database migrated successfully.");

            return serviceProvider;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}
