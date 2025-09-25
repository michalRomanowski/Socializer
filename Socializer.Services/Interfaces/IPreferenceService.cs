using Socializer.Database.Models;

namespace Socializer.Services.Interfaces;

public interface IPreferenceService
{
    Task<IEnumerable<Preference>> GetOrAddAsync(IEnumerable<Preference> preferences);
}
