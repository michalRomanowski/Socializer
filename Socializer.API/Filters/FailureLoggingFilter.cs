using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Socializer.API.Filters;

public class FailureLoggingFilter(ILogger<FailureLoggingFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.StatusCode >= 400)
        {
            var body = JsonSerializer.Serialize(objectResult.Value);
            logger.LogWarning(
                "Request: {Path} FAILED. Body: {reason}", context.HttpContext.Request.Path, body);
        }

        await next();
    }
}
