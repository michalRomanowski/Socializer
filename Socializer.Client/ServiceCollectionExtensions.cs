using Common.Client;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using Polly;
using Polly.Extensions.Http;
using Socializer.Client.ChatClient;
using Socializer.Shared;

namespace Socializer.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSocializerClient(this IServiceCollection services, SharedSettings settings)
    {
        services.AddScoped<ISocializerClient, SocializerClient>();

        // OpenIddict 
        services.AddOpenIddict()
            .AddClient(options =>
            {
                options.AddRegistration(new OpenIddictClientRegistration
                {
                    Issuer = new Uri(settings.SocializerApiUrl),
                    ClientId = "socializer-client-id",
                    GrantTypes = { OpenIddictConstants.GrantTypes.Password, OpenIddictConstants.GrantTypes.RefreshToken },
                    Scopes = { OpenIddictConstants.Scopes.OfflineAccess }
                });
                options.UseSystemNetHttp();
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();
            });

        // Poly retries
        services.AddHttpClient(ClientNames.WithRetries)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        3,
                        retryAttempt => TimeSpan.FromSeconds(5 + Math.Pow(2, retryAttempt)), // exponential backoff;
                        (result, delay, retryCount, context) => {
                            Console.WriteLine($"Retry {retryCount} in {delay.TotalSeconds}s due to {result.Exception?.Message ?? result.Result?.StatusCode.ToString()}");
                        }));

        return services;
    }

    public static IServiceCollection AddChatConnectionClient(this IServiceCollection services, SharedSettings settings)
    {
        services.AddScoped<IChatConnectionClient, SignalRChatConnectionClient>();

        return services;
    }
}
