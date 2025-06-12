using Microsoft.AspNetCore.Mvc;

namespace Socializer.API.Auth;

public class TokenRequestDtoAspMvc
{
    [FromForm(Name = "grant_type")]
    public string GrantType { get; set; }

    [FromForm(Name = "username")]
    public string Username { get; set; }

    [FromForm(Name = "password")]
    public string Password { get; set; }

    [FromForm(Name = "client_id")]
    public string ClientId { get; set; }
}
