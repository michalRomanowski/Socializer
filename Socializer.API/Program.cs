using Microsoft.EntityFrameworkCore;
using Socializer.API.Auth;
using Socializer.API.Filters;
using Socializer.API.Middleware;
using Socializer.API.Services;
using Socializer.API.Services.Interfaces;
using Socializer.API.Services.Services;
using Socializer.API.SignalR;
using Socializer.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SocializerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SocializerConnectionString")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FailureLoggingFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(SocializerAutomapperProfile));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();

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

// Seed db with single client
using (var scope = app.Services.CreateScope())
{
    await OpenIddictSeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.Run();
