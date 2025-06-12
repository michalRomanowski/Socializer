using Polly;
using Polly.Retry;
using Socializer.Shared;
using System.Net.Http.Json;

namespace Common.Client;

/// <summary>
/// Client for retrieving mobile app settings from a remote configuration server.
/// Has to be like that as init before DI as it's values used in DI.
/// </summary>
public static class ConfigClient
{
    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =
        Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(6, retryAttempt =>
                TimeSpan.FromSeconds(5 * retryAttempt));

    public static async Task<MobileAppSettings> GetMobileAppSettings(string appsettingsUrl)
    {
        using var client = new HttpClient();

        try
        {
            var response = await RetryPolicy.ExecuteAsync(() =>
                client.GetAsync(appsettingsUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode ?
                await response.Content.ReadFromJsonAsync<MobileAppSettings>() : // TODO: Should be generic, not relying on MobileAppSettings or anything Socializer related
                throw new Exception("Unable to retrieve configuration from server");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw;
        }
    }
}
