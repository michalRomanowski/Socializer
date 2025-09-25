using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socializer.Services.Interfaces;

namespace Socializer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPreferencesController(IUserPreferenceService userPreferencesService) : SocializerControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("My")]
    public async Task<IActionResult> GetPreferences()
    {
        var result = await userPreferencesService.GetAsync(UserId);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("My")]
    public async Task<IActionResult> DeletePreference(Guid userPreferenceId)
    {
        await userPreferencesService.DeleteAsync(UserId, userPreferenceId);
        return Ok();
    }
}
