using Socializer.API.Services.Services;

namespace Socializer.API.Services.Interfaces
{
    public interface IUserMatchingService
    {
        Task<IEnumerable<UserMatch>> UserMatchesAsync(string username);
    }
}
