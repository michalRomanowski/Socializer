using Microsoft.EntityFrameworkCore;
using Socializer.API.Auth;
using Socializer.API.Filters;
using Socializer.API.Middleware;
using Socializer.API.Services;
using Socializer.API.Services.Interfaces;
using Socializer.API.Services.Services;
using Socializer.Chat;
using Socializer.Chat.Extensions;
using Socializer.Database;
using Socializer.LLM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SocializerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SocializerConnectionString")));

//builder.Services.AddSingleton(
//    builder.Configuration.GetSection(nameof(HuggingFaceSettings)).Get<HuggingFaceSettings>());

builder.Services.AddSingleton(
    builder.Configuration.GetSection(nameof(TogetherAISettings)).Get<TogetherAISettings>());

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FailureLoggingFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(SocializerAutomapperProfile));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();
builder.Services.AddScoped<IUserPreferenceService, UserPreferenceService>();
builder.Services.AddScoped<IUserMatchingService, UserMatchingService>();
builder.Services.AddChat();
//builder.Services.AddTransient<ILLMClient, HuggingFaceClient>();
builder.Services.AddTransient<ILLMClient, TogetherAISocializerClient>();

var app = builder.Build();

app.UseMiddleware<RequestMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.Services.MigrateDatabase(app.Logger);

// Seed db with single client
using (var scope = app.Services.CreateScope())
{
    await OpenIddictSeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.Run();
