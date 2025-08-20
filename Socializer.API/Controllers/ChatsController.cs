using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socializer.Services.Interfaces.Chat;

namespace Socializer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatsController(IChatService chatService) : SocializerControllerBase
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("My")]
    public async Task<IActionResult> GetChats()
    {
        var result = await chatService.GetChatsAsync(UserId);

        return Ok(result);
    }
}