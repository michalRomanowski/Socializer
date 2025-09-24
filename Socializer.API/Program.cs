using Auth.API;
using Auth.Database;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Socializer.API.Middleware;
using Socializer.Database;
using Socializer.LLM;
using Socializer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SocializerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SocializerConnectionString"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddControllers();

builder.Services.AddSingleton(
    builder.Configuration.GetSection(nameof(TogetherAISettings)).Get<TogetherAISettings>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(SocializerAutomapperProfile));
builder.Services.AddLLM();
builder.Services.AddServices();

// Configure and use Serilog to file
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddleware<RequestMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.MigrateSocializerDatabase(app.Logger);
app.Services.MigrateAuthDatabase(app.Logger);

// Seed db with single client
using (var scope = app.Services.CreateScope())
{
    await OpenIddictSeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.Run();
