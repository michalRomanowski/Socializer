using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.LLM;

namespace Socializer.API.Services.Services;

public class PreferenceService(ILLMClient llmClient, SocializerDbContext dbContext, ILogger<PreferenceService> logger) : IPreferenceService
{
    public async Task<Preference> GetOrAddAsync(Preference preference)
    {
        var existingPreference =
            await dbContext.Preferences.SingleOrDefaultAsync(x => x.DBPediaResource == preference.DBPediaResource);

        if (existingPreference == null)
        {
            dbContext.Preferences.Add(preference);
        }
        else
        {
            preference = existingPreference;
        }

        dbContext.SaveChanges();
        return preference;
    }

    public async Task<IEnumerable<Preference>> ExtractPreferencesAsync(string prompt)
    {
        var preferencesPrompt = Prompts.PreferencesPrompt(prompt);

        logger.LogDebug("Preferences prompt: {PreferencesPrompt}", preferencesPrompt);

        var response = await llmClient.QueryAsync(preferencesPrompt);

        var preferences = ReadPreferences(response);

        logger.LogInformation("Extracted {PreferencesCount} preferences.", preferences.Count);

        return preferences;
    }

    private List<Preference> ReadPreferences(string csv)
    {
        var preferences = new List<Preference>();

        var lines = csv.Split('\n');

        logger.LogDebug("Extracted {LinesCount} lines.", lines.Length);

        foreach (var line in lines)
        {
            logger.LogDebug("Extracting preferences from line {Line}.", line);

            try
            {
                var split = line.Split(',');

                var type = (EPreferenceType)Enum.Parse(typeof(EPreferenceType), split[0], true);
                var link = split[1];

                preferences.Add(
                    new Preference()
                    {
                        PreferenceType = type,
                        DBPediaResource = link,
                    });
            }
            catch(Exception ex) // Log and skip failed lines
            {
                logger.LogWarning("Failed extracting preference from line {Line}. Exception Message: {ExceptionMessage}", line, ex.Message);
            }
        }

        return preferences;
    }
}
