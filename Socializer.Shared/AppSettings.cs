namespace Socializer.Shared;

public class AppSettings
{
    public LoggingSettings Logging { get; set; } = new();
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public AuthSettings Auth { get; set; } = new();
    public MobileAppSettings MobileAppSettings { get; set; } = new();
}

public class LoggingSettings
{
    public LogLevelSettings LogLevel { get; set; } = new();
}

public class LogLevelSettings
{
    public string Default { get; set; } = string.Empty;
    public string MicrosoftAspNetCore { get; set; } = string.Empty;
}

public class ConnectionStrings
{
    public string SocializerConnectionString { get; set; } = string.Empty;
    public string IdentityConnectionString { get; set; } = string.Empty;
}

public class AuthSettings
{
    public string ResourceServerName { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}

/// <summary>
/// Settings specific to the mobile app
/// </summary>
public class MobileAppSettings
{
    public string SocializerApiUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}
