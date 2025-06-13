using Common.Client;
using Common.Utils;
using Socializer.Shared.Dtos;

namespace Socializer.Client;

public class SocializerClient(IClient client) : ISocializerClient
{
    public async Task<OperationResult<bool>> LoginAsync(string username, string password)
    {
        return await client.LoginAsync(username, password);
    }

    public async Task<OperationResult<UserDto>> GetUserMeAsync()
    {
        return await client.GetAsync<UserDto>("Users/Me");
    }

    public async Task<OperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var createUserResult = await client.PostAsync("Users", createUserDto);

        if (!createUserResult.IsSuccess)
        {
            return createUserResult;
        }

        var loginResult = await client.LoginAsync(createUserDto.Username, createUserDto.Password);

        if (!loginResult.IsSuccess)
        {
            return OperationResult<CreateUserDto>.Failure(loginResult.Errors);
        }

        return createUserResult;
    }
}
