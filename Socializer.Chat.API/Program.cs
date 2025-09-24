using Auth.API;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Serilog;
using Socializer.Chat;
using Socializer.Chat.API.Filters;
using Socializer.Chat.Extensions;
using Socializer.Database;
using Socializer.Database.NoSql;
using Socializer.Database.NoSql.Extensions;
using Socializer.LLM;
using Socializer.Repository;
using Socializer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SocializerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SocializerConnectionString"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddSingleton(
    builder.Configuration.GetSection(nameof(OpenAISettings)).Get<OpenAISettings>());

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FailureLoggingFilter>();
});

builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(SocializerAutomapperProfile));

builder.Services.AddChat();
builder.Services.AddLLM();
builder.Services.AddServices();
builder.Services.AddTableServiceClient(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddSingleton<TableStorageInitializer>();

// Configure and use Serilog to file
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

//if (!app.Environment.IsDevelopment())
app.UseHttpsRedirection();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");

await app.Services.GetService<TableStorageInitializer>()!.CreateTablesIfNotExistAsync();

app.Run();
