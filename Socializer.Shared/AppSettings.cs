namespace Socializer.Shared;

/// <summary>
/// Settings shared between server and mobile app
/// </summary>
public class SharedSettings
{
    public string SocializerApiUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}
