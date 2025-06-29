using Socializer.Database.Models;

namespace Socializer.API.Services.Interfaces;

public interface IPreferenceService
{
    Task<IEnumerable<Preference>> GetPreferencesAsync(string prompt);
}
