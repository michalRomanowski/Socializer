using Socializer.Database.Models;

namespace Socializer.API.Services.Interfaces;

public interface IUserPreferenceService
{
    Task<IEnumerable<UserPreference>> GetAsync(string username);
    Task<UserPreference> UpdateOrAddAsync(string username, Preference preference);
}
