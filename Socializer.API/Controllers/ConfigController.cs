using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socializer.Shared;

namespace Socializer.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class ConfigController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var section = configuration.GetSection("SharedSettings");
        var sharedSettings = section.Get<SharedSettings>();

        return Ok(sharedSettings);
    }
}
