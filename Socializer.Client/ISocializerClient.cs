using Common.Utils;
using Socializer.Shared.Dtos;

namespace Socializer.Client;

public interface ISocializerClient
{
    Task<OperationResult<bool>> LoginAsync(string username, string password);
    Task<OperationResult<UserDto>> GetUserMeAsync();
    Task<OperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto createUserDto);
}
