using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socializer.Services.Interfaces;

namespace Socializer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PreferencesController(IPreferenceService preferencesService) : SocializerControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("My")]
    public async Task<IActionResult> GetPreferences()
    {
        var result = await preferencesService.GetAsync(UserId);
        return Ok(result);
    }
}
