using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Socializer.API.Auth;
using Socializer.Database.Models;

namespace Socializer.API.Controllers;

[Controller]
[Route("connect/token")]
public class AuthTokenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Exchange([FromForm] TokenRequestDtoAspMvc tokenRequest)
    {
        return tokenRequest.GrantType switch
        {
            OpenIddictConstants.GrantTypes.Password => await ExchangePassword(tokenRequest.Username, tokenRequest.Password),
            OpenIddictConstants.GrantTypes.RefreshToken => await ExchangeRefreshToken(),
            _ => NotFound(),
        };
    }

    private async Task<IActionResult> ExchangePassword(string username, string password)
    {
        var user = await userManager.FindByNameAsync(username);

        if (user == null || !await userManager.CheckPasswordAsync(user, password))
            return NotFound();

        var principal = await signInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(
            new[]
            {
                OpenIddictConstants.Scopes.OfflineAccess // Allows issuing a refresh
            }
        );

        principal.SetClaim(OpenIddictConstants.Claims.Subject, user.Id);
        principal.SetResources(configuration["AuthSettings:ResourceServerName"]);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> ExchangeRefreshToken()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        var refreshPrincipal = result.Principal;

        // Optionally regenerate or update claims here
        refreshPrincipal.SetResources(configuration["AuthSettings:ResourceServerName"]);

        return SignIn(refreshPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
