using Common.Client;
using Microsoft.Extensions.Logging;
using Socializer.BlazorHybrid.Services;
using Socializer.Client;

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
            var mobileAppSettings = ConfigClient.GetSharedSettings(Constants.ConfigUrl).Result;

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped((services) => mobileAppSettings);
            builder.Services.AddScoped<IClient, OpenIddictClient>();
            builder.Services.AddSocializerClient(mobileAppSettings);
            builder.Services.AddChatConnectionClient(mobileAppSettings);
            builder.Services.AddScoped<LayoutState>();
            builder.Services.AddScoped<StateContainer>();
            builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
