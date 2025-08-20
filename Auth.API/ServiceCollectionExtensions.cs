using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Socializer.Auth.Database;
using System.Security.Cryptography.X509Certificates;

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

        services.AddIdentity<IdentityUser, IdentityRole>()
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

                var signInCert = new X509Certificate2(
                    "C:\\Certs\\SocializerOpenIddictSignInCert.pfx",
                    Environment.GetEnvironmentVariable("SocializerOpenIddictSignInCert_PASSWORD"));

                options.AddSigningCertificate(signInCert);

                options.DisableAccessTokenEncryption(); // Disable only for Access token

                var encryptionCert = new X509Certificate2(
                    "C:\\Certs\\SocializerOpenIddictEncryptionCert.pfx",
                    Environment.GetEnvironmentVariable("SocializerOpenIddictEncryptionCert_PASSWORD"));

                options.AddEncryptionCertificate(encryptionCert);

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
                options.Authority = configuration["SharedSettings:SocializerApiUrl"];
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidAudience = configuration["AuthSettings:ResourceServerName"]
                };

                options.Events = new JwtBearerEvents
                {
                    // Config for SignalR chat
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/chathub")) // TODO: Move to some const as also used in auth configuration, now hardcoded in both places
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
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
