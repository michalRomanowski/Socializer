using Common.Client;
using Microsoft.Extensions.Logging;
using Socializer.BlazorHybrid.Services;
using Socializer.BlazorShared.Extensions;

namespace Socializer.BlazorHybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Only used in startup so no need to register MobileAppSettings yet, maybe in future if needed
            var sharedSettings = ConfigClient.GetSharedSettings(Constants.ConfigUrl).Result;

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped((services) => sharedSettings);

            builder.Services.AddScoped<Common.Utils.ISecureStorage, MauiSecureStorage>();
            builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();

            builder.Services.AddBlazorShared(sharedSettings);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
