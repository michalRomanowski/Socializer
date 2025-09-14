using Microsoft.Extensions.Logging;
using Socializer.Database.Models;
using Socializer.Services.Interfaces;

namespace Socializer.Services.Services;

internal class ReadPreferencesService(ILogger<ReadPreferencesService> logger) : IReadPreferencesService
{
    private readonly static StringSplitOptions stringSplitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

    public IEnumerable<Preference> ReadPreferences(string text)
    {
        var preferences = new List<Preference>();

        var lines = text.Split('\n', stringSplitOptions).Distinct();

        logger.LogDebug("Extracted {LinesCount} lines.", lines.Count());

        foreach (var line in lines)
        {
            logger.LogDebug("Extracting preferences from line {Line}.", line);

            try
            {
                preferences.Add(
                    new Preference()
                    {
                        DBPediaResource = line,
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