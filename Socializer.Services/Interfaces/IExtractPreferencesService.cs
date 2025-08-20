using Socializer.Database.Models;

namespace Socializer.Services.Interfaces;

public interface IExtractPreferencesService
{
    Task<IEnumerable<Preference>> ExtractPreferencesAsync(string prompt);
}
