using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socializer.API.Services.Interfaces;
using Socializer.Shared.Dtos;

namespace Socializer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : SocializerControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserDto createUserDto)
    {
        var operationResult = await userService.CreateUserAsync(createUserDto);

        return operationResult.IsSuccess ?
            Created(
                string.Empty, // TODO: reconsider
                operationResult.Result) :
            BadRequest(operationResult.Errors);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("Me")]
    public async Task<IActionResult> Get()
    {
        var result = await userService.GetUserAsync(UserId);

        return Ok(result);
    }
}
