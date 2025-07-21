using Socializer.Database.Models;

namespace Socializer.API.Services.Interfaces;

public interface IUserPreferenceService
{
    Task<IEnumerable<UserPreference>> GetAsync(Guid userId);
    Task<IEnumerable<UserPreference>> AddOrUpdateAsync(Guid userId, IEnumerable<Preference> preferences);
}
