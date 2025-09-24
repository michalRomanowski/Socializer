using Microsoft.Extensions.DependencyInjection;

namespace Socializer.LLM;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLLM(this IServiceCollection services)
    {
        services.AddTransient<ILLMClient, OpenAISocializerClient>();

        return services;
    }
}
