using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database.Models;
using Socializer.LLM;

namespace Socializer.API.Services.Services;

public class PreferenceService(ILLMClient llmClient, ILogger<PreferenceService> logger) : IPreferenceService
{
    public async Task<IEnumerable<Preference>> GetPreferencesAsync(string prompt)
    {
        var preferencesPrompt = Prompts.PreferencesPrompt(prompt);

        logger.LogDebug("Preferences prompt: {PreferencesPrompt}", preferencesPrompt);

        var response = await llmClient.QueryAsync(preferencesPrompt);

        var preferences = ExtractPreferences(response);

        logger.LogInformation("Extracted {PreferencesCount} preferences.", preferences.Count);

        return preferences;
    }

    private List<Preference> ExtractPreferences(string csv)
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
