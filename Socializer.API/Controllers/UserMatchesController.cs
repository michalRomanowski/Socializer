using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socializer.API.Services.Interfaces;

namespace Socializer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserMatchesController(IUserMatchingService userMatchingService) : SocializerControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("My")]
    public async Task<IActionResult> GetMatches()
    {
        var result = await userMatchingService.UserMatchesAsync(UserId);

        return Ok(result);
    }
}