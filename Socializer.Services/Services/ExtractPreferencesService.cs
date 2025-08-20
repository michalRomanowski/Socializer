using Microsoft.Extensions.Logging;
using Socializer.Database.Models;
using Socializer.LLM;
using Socializer.Services.Interfaces;

namespace Socializer.Services.Services;

public class ExtractPreferencesService(ILLMClient llmClient, ILogger<ExtractPreferencesService> logger) : IExtractPreferencesService
{
    public async Task<IEnumerable<Preference>> ExtractPreferencesAsync(string prompt)
    {
        var preferencesPrompt = Prompts.PreferencesPrompt(prompt, 100);

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