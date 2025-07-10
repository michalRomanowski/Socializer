using Socializer.Database.Models;

namespace Socializer.API.Services.Interfaces;

public interface IPreferenceService
{
    Task<Preference> GetOrAddAsync(Preference preference);
    Task<IEnumerable<Preference>> ExtractPreferencesAsync(string prompt);
}
