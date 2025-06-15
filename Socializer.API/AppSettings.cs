using Socializer.Shared;

namespace Socializer.API;

public class AppSettings
{
    public LoggingSettings Logging { get; set; } = new();
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public AuthSettings Auth { get; set; } = new();
    public SharedSettings MobileAppSettings { get; set; } = new();
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