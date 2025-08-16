namespace Socializer.API.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "WasmAndMobile";

    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy
                    .WithOrigins(
                        "https://localhost:443", // dev TODO: Check if unsafe to leave it like that
                        "https://api.socializerapi.eu") // prod
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void UseCors(this WebApplication app)
    {
        app.UseCors(CorsPolicyName);
    }
}
