using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Auth.API;

internal static class JwtBearerOptionsSignalRExtensions
{

    public static void AddSignalROptions(this JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            // Config for SignalR chat
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chathub")) // TODO: Move to some const as also used in auth configuration, now hardcoded in both places
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    }
}