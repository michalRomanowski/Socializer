using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database.Models;
using Socializer.LLM;
using System.Text;

namespace Socializer.API.Services.Services;

public class PreferenceService(ILLMClient llmClient, ILogger<PreferenceService> logger) : IPreferenceService
{
    private static readonly string preferenceTypesText = string.Join(", ", Enum.GetNames(typeof(EPreferenceType)).Select(x => $"\"{x}\""));

    public async Task<IEnumerable<Preference>> GetPreferencesAsync(string prompt)
    {
        var preferencesPrompt = FormatPreferencesPrompt(prompt);

        logger.LogDebug("Preferences prompt: {PreferencesPrompt}", preferencesPrompt);

        var response = await llmClient.QueryAsync(preferencesPrompt);

        var preferences = ExtractPreferences(response);

        logger.LogInformation("Extracted {PreferencesCount} preferences.", preferences.Count);

        return preferences;
    }

    private static string FormatPreferencesPrompt(string prompt)
    {
        var preferencesPrompt = new StringBuilder("From this text:");
        preferencesPrompt.AppendLine($"\"{prompt}\"");
        // TODO: build list of properties from EPreferenceType
        preferencesPrompt.AppendLine($"Extract properties corresponding to text of types: {preferenceTypesText}, for each property add link to http://dbpedia.org.");
        preferencesPrompt.AppendLine("Return only list in form:");
        preferencesPrompt.AppendLine("type, link");
        preferencesPrompt.AppendLine("and nothing else.");

        return preferencesPrompt.ToString();
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
                        Url = link,
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
