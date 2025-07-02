using Common.Utils;
using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.API.Services.Interfaces;

public interface IUserService
{
    Task<OperationResult<UserDto>> GetUserAsync(Guid userId);
    Task<OperationResult<CreateUserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<User> AddPreferencesAsync(string username, IEnumerable<Preference> preferences); // TODO: Could return OperationResult
}
