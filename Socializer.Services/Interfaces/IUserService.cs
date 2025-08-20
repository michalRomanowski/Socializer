using Common.Utils;
using Socializer.Shared.Dtos;

namespace Socializer.Services.Interfaces;

public interface IUserService
{
    Task<string> GetUsernameAsync(Guid userId);
    Task<UserDto> GetUserAsync(Guid userId);
    Task<OperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto createUserDto);
}
