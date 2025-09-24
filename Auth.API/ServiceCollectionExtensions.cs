using Auth.Database;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace Auth.API;

public static class ServiceCollectionExtensions
{
    private const string SignInCertPasswordEnvVar = "SocializerOpenIddictSignInCert_PASSWORD";
    private const string EncryptionCertPasswordEnvVar = "SocializerOpenIddictEncryptionCert_PASSWORD";

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("IdentityConnectionString"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure())
                .UseOpenIddict());

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
                options.SetIssuer(configuration["SharedSettings:SocializerApiUrl"]);

                options.SetTokenEndpointUris("/connect/token");
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();
                options.AcceptAnonymousClients(); // For public clients like MAUI

                options.AddCertificates(configuration);

                options.DisableAccessTokenEncryption(); // Disable only for Access token

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

                options.AddSignalROptions();

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

    private static void AddCertificates(this OpenIddictServerBuilder options, IConfiguration configuration)
    {
        // Determine if running locally or in Azure Web App
        var isAzureWebApp = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));

        X509Certificate2 signInCert;
        X509Certificate2 encryptionCert;

        if (isAzureWebApp)
        {
            signInCert = LoadCertificateFromAzureKeyVault(
                configuration,
                "SocializerOpenIddictSignInCert",
                Environment.GetEnvironmentVariable(SignInCertPasswordEnvVar)!);

            encryptionCert = LoadCertificateFromAzureKeyVault(
                configuration,
                "SocializerOpenIddictEncryptionCert",
                Environment.GetEnvironmentVariable(EncryptionCertPasswordEnvVar)!);
        }
        else
        {
            signInCert = new X509Certificate2(
                "C:\\Certs\\SocializerOpenIddictSignInCert.pfx",
                Environment.GetEnvironmentVariable(SignInCertPasswordEnvVar));

            encryptionCert = new X509Certificate2(
                "C:\\Certs\\SocializerOpenIddictEncryptionCert.pfx",
                Environment.GetEnvironmentVariable(EncryptionCertPasswordEnvVar));
        }

        options.AddSigningCertificate(signInCert);
        options.AddEncryptionCertificate(encryptionCert);
    }

    private static X509Certificate2 LoadCertificateFromAzureKeyVault(IConfiguration configuration, string secretName, string password)
    {
        var secretClient = new SecretClient(
            new Uri(configuration["AuthSettings:KeyVaultUri"]!),
            new ManagedIdentityCredential());

        // Get the certificate with private key as a secret
        var certificateWithPrivateKey = secretClient.GetSecret(secretName);
        var privateKeyBytesSignIn = Convert.FromBase64String(certificateWithPrivateKey.Value.Value);

        var cert = new X509Certificate2(
            privateKeyBytesSignIn, 
            Environment.GetEnvironmentVariable(password), 
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

        return cert;
    }
}
