using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Socializer.Chat.API.Filters;

public class FailureLoggingFilter(ILogger<FailureLoggingFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.StatusCode >= 400)
        {
            var body = JsonSerializer.Serialize(objectResult.Value);
            logger.LogWarning(
                "Request: {Path} FAILED. Body: {reason}. TrackingId {trackingId}.", context.HttpContext.Request.Path, body, context.HttpContext.TraceIdentifier);
        }

        await next();
    }
}
