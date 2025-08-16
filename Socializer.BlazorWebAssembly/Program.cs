using Common.Utils;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Socializer.BlazorShared.Extensions;
using Socializer.BlazorWebAssembly;
using Socializer.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// TODO: Rework for different envs when ready
var sharedSettings = new SharedSettings()
{
    SocializerApiUrl = Constants.SocializerApiUrl
};

builder.Services.AddScoped<ISecureStorage, DummySecureStorage>();
builder.Services.AddScoped((services) => sharedSettings);
builder.Services.AddBlazorShared(sharedSettings);

await builder.Build().RunAsync();
