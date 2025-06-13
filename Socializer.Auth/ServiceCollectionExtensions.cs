//using Microsoft.AspNetCore.Authentication.JwtBearer; // UNCOMMENT FOR AUTH DEBUGGING
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Socializer.Database;
using Socializer.Database.Models;

namespace Socializer.API.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"));
            options.UseOpenIddict();
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        // Configure OpenIddict server side
        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<IdentityDbContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/connect/token");
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();
                options.AcceptAnonymousClients(); // For public clients like MAUI

                options.AddDevelopmentSigningCertificate();     // Sign tokens, TODO: replace with real cert for prod
                options.DisableAccessTokenEncryption();         // Disable only for Access token
                options.AddDevelopmentEncryptionCertificate();  // Other kinds of tokens still need encryption Cert TODO: replace with real cert for prod

                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        // Configure JWT Bearer server side, this must be in sync with OpenIddict above
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = configuration["Auth:Issuer"];
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidAudience = configuration["Auth:ResourceServerName"]
                };

                // UNCOMMENT FOR AUTH DEBUGGING
                //options.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = context =>
                //    {
                //        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                //        return Task.CompletedTask;
                //    },
                //    OnTokenValidated = context =>
                //    {
                //        Console.WriteLine("Token validated successfully.");
                //        return Task.CompletedTask;
                //    },
                //    OnMessageReceived = context =>
                //    {
                //        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                //        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                //        {
                //            context.Token = authHeader.Substring("Bearer ".Length);
                //        }

                //        return Task.CompletedTask;
                //    }
                //};
            });

        // UNCOMMENT FOR AUTH DEBUGGING
        //IdentityModelEventSource.ShowPII = true;

        services.AddAuthorization();

        return services;
    }
}
