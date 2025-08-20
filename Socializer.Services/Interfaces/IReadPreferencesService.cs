using Socializer.Database.Models;

namespace Socializer.Services.Interfaces;

public interface IReadPreferencesService
{
    IEnumerable<Preference> ReadPreferences(string text);
}
