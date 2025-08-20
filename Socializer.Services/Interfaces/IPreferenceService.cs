using Socializer.Database.Models;

namespace Socializer.API.Services.Interfaces;

public interface IPreferenceService
{
    Task<IEnumerable<Preference>> GetOrAddAsync(IEnumerable<Preference> preferences);
    Task<IEnumerable<Preference>> ExtractPreferencesAsync(string prompt);
}
