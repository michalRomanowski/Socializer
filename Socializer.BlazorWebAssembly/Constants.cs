namespace Socializer.BlazorWebAssembly;

internal class Constants
{
#if DEBUG
    public static readonly string SocializerApiUrl = "http://localhost:80";
#else
    public static readonly string SocializerApiUrl = "https://api.socializerapi.eu";
#endif
}
