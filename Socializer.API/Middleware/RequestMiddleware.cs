namespace Socializer.API.Middleware;

public class RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var path = context.Request.Path;

            // TODO: Tracking Id should be attached to every log. This is doable.
             
            logger.LogInformation(
                    "Request: {Path} received. TrackingId {trackingId}.",
                    path, context.TraceIdentifier);

            await next(context);

            var statusCode = context.Response.StatusCode;

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
                ex, "Request: {Path} FAILED UNHANDLED. TrackingId {trackingId}.", context.Request.Path, context.TraceIdentifier);

            var errorResponse = new
            {
                Message = "Unexpected error.",
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}