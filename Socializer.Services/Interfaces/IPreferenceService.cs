using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.Services.Interfaces;

public interface IPreferenceService
{
    Task<IEnumerable<PreferenceDto>> GetAsync(Guid userId);
    Task<IEnumerable<Preference>> GetOrAddAsync(IEnumerable<Preference> preferences);
}
