using Polly;
using Polly.Retry;
using Socializer.Shared;
using System.Net.Http.Json;

namespace Common.Client;

/// <summary>
/// Client for retrieving settings for mobile app from a remote configuration server.
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

    public static async Task<SharedSettings> GetSharedSettings(string appsettingsUrl)
    {
        using var client = new HttpClient();

        try
        {
            var response = await RetryPolicy.ExecuteAsync(() =>
                client.GetAsync(appsettingsUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode ?
                await response.Content.ReadFromJsonAsync<SharedSettings>() :
                throw new Exception("Unable to retrieve configuration from server");
        }
        catch (Exception)
        {
            // TODO: Log the exception or handle it as needed
            throw;
        }
    }
}
