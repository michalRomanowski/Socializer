using Socializer.Shared.Dtos;

namespace Socializer.Services.Interfaces;

public interface IUserMatchingService
{
    Task<IEnumerable<UserMatchDto>> UserMatchesAsync(Guid userId);
}
