using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace Socializer.BlazorHybrid
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();

            // For keyboard behavior not overshadowing the input field in Android
            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            MainPage = new MainPage();
        }
    }
}
