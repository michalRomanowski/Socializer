using Common.Utils;
using Socializer.Shared.Dtos;

namespace Socializer.API.Services.Interfaces;

public interface IUserService
{
    Task<OperationResult<UserDto>> GetUserAsync(Guid userId);
    Task<OperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto createUserDto);
}
