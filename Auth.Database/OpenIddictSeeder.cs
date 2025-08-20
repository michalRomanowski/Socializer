using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace Auth.Database;

public static class OpenIddictSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var clientId = "socializer-client-id";

        if (await manager.FindByClientIdAsync(clientId) == null)
        {
            await manager.CreateAsync
            (
                new OpenIddictApplicationDescriptor
                {
                    ClientId = clientId,
                    DisplayName = "Swagger, Mobile and Dev Client",
                    ClientType = OpenIddictConstants.ClientTypes.Public, // No client secret required for public client (typical for mobile apps)

                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.Prefixes.Scope + "offline_access"
                    }
                }
            );
        }
    }
}