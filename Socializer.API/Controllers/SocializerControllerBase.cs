using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Socializer.API.Controllers;

public class SocializerControllerBase() : ControllerBase
{
    protected Guid UserId => new(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Cannot retrieve authenticated user"));
}
