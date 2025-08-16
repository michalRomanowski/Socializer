using Common.Utils;
using OpenIddict.Client;
using Socializer.Shared;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static OpenIddict.Client.OpenIddictClientModels;

namespace Common.Client;

public class OpenIddictClient(
    OpenIddictClientService clientService,
    ISecureStorage secureStorage,
    IHttpClientFactory httpClientFactory,
    SharedSettings sharedSettings) : IClient
{
    public async Task<OperationResult<TDto>> GetAsync<TDto>(string urlPath)
    {
        try
        {
            using var httpClient = await GetHttpClientAsync();
            await RefreshTokenAsync();

            var response = await httpClient.GetAsync($"{sharedSettings.SocializerApiUrl}/{urlPath}");

            return await OperationResult<TDto>.FromHttpResponseAsync(response);
        }
        catch (Exception ex)
        {
            return OperationResult<TDto>.Failure(ex);
        }
    }

    public async Task<OperationResult<TDto>> PostAsync<TDto>(string urlPath, TDto dto)
    {
        var json = JsonSerializer.Serialize(dto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            using var httpClient = await GetHttpClientAsync();
            await RefreshTokenAsync();

            var response = await httpClient.PostAsync($"{sharedSettings.SocializerApiUrl}/{urlPath}", content);
            return await OperationResult<TDto>.FromHttpResponseAsync(response);
        }
        catch (Exception ex)
        {
            return OperationResult<TDto>.Failure(ex);
        }
    }

    public async Task<OperationResult<bool>> LoginAsync(string username, string password)
    {
        try
        {
            var result = await clientService.AuthenticateWithPasswordAsync(new PasswordAuthenticationRequest() { Username = username, Password = password });

            await secureStorage.SetAsync("access_token", result.AccessToken);
            await secureStorage.SetAsync("refresh_token", result.RefreshToken);
            await secureStorage.SetAsync("auth_access_token_expires_at", result.AccessTokenExpirationDate?.ToString("o")); // ISO 8601

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }

    public async Task<OperationResult<bool>> LoginAsync()
    {
        try
        {
            if (await IsAccessTokenExpired() == false)
                return OperationResult<bool>.Success(true);

            return await RefreshTokenAsync();
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }

    private async Task<OperationResult<bool>> RefreshTokenAsync()
    {
        try
        {
            var accessToken = await secureStorage.GetAsync("access_token");

            if (!await IsAccessTokenExpired())
                return OperationResult<bool>.Success(true);

            var refreshToken = await secureStorage.GetAsync("refresh_token");

            var result = await clientService.AuthenticateWithRefreshTokenAsync(
                new RefreshTokenAuthenticationRequest
                {
                    RefreshToken = refreshToken
                });

            await secureStorage.SetAsync("access_token", result.AccessToken);
            await secureStorage.SetAsync("refresh_token", result.RefreshToken);
            await secureStorage.SetAsync("auth_access_token_expires_at", result.AccessTokenExpirationDate?.ToString("o")); // ISO 8601

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }

    private async Task<HttpClient> GetHttpClientAsync()
    {
        var httpClient = httpClientFactory.CreateClient(ClientNames.WithRetries);
        var token = await secureStorage.GetAsync("access_token");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return httpClient;
    }

    private async Task<bool> IsAccessTokenExpired()
    {
        var expiry = DateTime.Parse(
            await secureStorage.GetAsync("auth_access_token_expires_at"),
            null,
            DateTimeStyles.RoundtripKind); // DateTimeStyles.RoundtripKind corresponds to string format "o"

        return expiry.ToUniversalTime() <= DateTime.UtcNow.AddMinutes(5); // 5 minutes before expiration considered as expired. TODO: can be configurable
    }
}
