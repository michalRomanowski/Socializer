using Common.Client;
using Microsoft.Extensions.DependencyInjection;
using Socializer.Client;
using Socializer.Shared;

namespace Socializer.BlazorShared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorShared(this IServiceCollection services, SharedSettings sharedSettings)
    {
        services.AddScoped<IClient, OpenIddictClient>();
        services.AddSocializerClient(sharedSettings);
        services.AddChatConnectionClient(sharedSettings);

        services.AddScoped<LayoutState>();
        services.AddScoped<StateContainer>();

        return services;
    }
}
