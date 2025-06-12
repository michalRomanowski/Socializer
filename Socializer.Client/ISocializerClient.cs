using Common.Client;
using Common.Utils;
using Socializer.Shared.Dtos;

namespace Socializer.Client;

public interface ISocializerClient
{
    Task<OperationResult<bool>> LoginAsync(string username, string password);
    Task<ClientOperationResult<UserDto>> GetUserMeAsync();
    Task<ClientOperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto createUserDto);
}
