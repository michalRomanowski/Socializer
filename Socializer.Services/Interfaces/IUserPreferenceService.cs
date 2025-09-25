using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.Services.Interfaces;

public interface IUserPreferenceService
{
    Task<IEnumerable<UserPreferenceDto>> GetAsync(Guid userId);
    Task<IEnumerable<UserPreference>> AddOrUpdateAsync(Guid userId, IEnumerable<Preference> preferences);
    Task DeleteAsync(Guid userId, Guid userPreferenceId);
}
