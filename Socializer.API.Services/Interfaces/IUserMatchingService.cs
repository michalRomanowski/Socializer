using Common.Utils;
using Socializer.Shared.Dtos;

namespace Socializer.API.Services.Interfaces;

public interface IUserMatchingService
{
    Task<OperationResult<IEnumerable<UserMatchDto>>> UserMatchesAsync(Guid userId);
}
