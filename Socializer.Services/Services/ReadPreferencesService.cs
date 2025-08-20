using Microsoft.Extensions.Logging;
using Socializer.Database.Models;
using Socializer.Services.Interfaces;

namespace Socializer.Services.Services;

internal class ReadPreferencesService(ILogger<ReadPreferencesService> logger) : IReadPreferencesService
{
    public IEnumerable<Preference> ReadPreferences(string text)
    {
        var preferences = new List<Preference>();

        var lines = text.Split('\n');

        logger.LogDebug("Extracted {LinesCount} lines.", lines.Length);

        foreach (var line in lines)
        {
            logger.LogDebug("Extracting preferences from line {Line}.", line);

            try
            {
                var dbPediaResource = line.Trim();

                preferences.Add(
                    new Preference()
                    {
                        DBPediaResource = dbPediaResource,
                    });
            }
            catch (Exception ex) // Log and skip failed lines
            {
                logger.LogWarning("Failed extracting preference from line {Line}. Exception Message: {ExceptionMessage}", line, ex.Message);
            }
        }

        return preferences;
    }
}