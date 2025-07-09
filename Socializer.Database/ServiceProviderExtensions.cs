using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Socializer.Database
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider MigrateDatabase(this IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();

                var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                identityDbContext.Database.Migrate();
                logger.LogInformation("Identity Database migrated successfully.");

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
}
