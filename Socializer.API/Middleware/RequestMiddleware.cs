namespace Socializer.API.Middleware;

public class RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);

            var statusCode = context.Response.StatusCode;
            var path = context.Request.Path;

            if (statusCode >= 200 && statusCode <= 299)
            {
                logger.LogInformation(
                    "Request: {Path} completed successfully. StatusCode: {StatusCode}. TrackingId {trackingId}.", 
                    path, statusCode, context.TraceIdentifier);
            }
            else
            {
                logger.LogWarning(
                    "Request: {Path} FAILED. StatusCode: {StatusCode}. TrackingId {trackingId}.", 
                    path, statusCode, context.TraceIdentifier);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, "Request: {Path}. TrackingId {trackingId}.", context.Request.Path, context.TraceIdentifier);

            var errorResponse = new
            {
                Message = "Unexpected error.",
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}