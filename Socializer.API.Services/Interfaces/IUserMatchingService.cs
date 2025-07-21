using Socializer.Shared.Dtos;

namespace Socializer.API.Services.Interfaces;

public interface IUserMatchingService
{
    Task<IEnumerable<UserMatchDto>> UserMatchesAsync(Guid userId);
}
