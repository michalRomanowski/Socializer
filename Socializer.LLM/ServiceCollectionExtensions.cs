using Microsoft.Extensions.DependencyInjection;

namespace Socializer.LLM;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLLM(this IServiceCollection services)
    {
        //services.AddTransient<ILLMClient, HuggingFaceClient>();
        services.AddTransient<ILLMClient, TogetherAISocializerClient>();

        return services;
    }
}
