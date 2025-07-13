using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.LLM;

namespace Socializer.API.Services.Services;

public class PreferenceService(ILLMClient llmClient, SocializerDbContext dbContext, ILogger<PreferenceService> logger) : IPreferenceService
{
    public async Task<IEnumerable<Preference>> GetOrAddAsync(IEnumerable<Preference> preferences)
    {
        var resources = preferences.Select(x => x.DBPediaResource).ToHashSet();

        var existingPreferences = await dbContext.Preferences
            .Where(x => resources.Contains(x.DBPediaResource))
            .AsNoTracking()
            .ToListAsync();

        var existingResources = existingPreferences.Select(x => x.DBPediaResource);

        var preferencesToAdd = preferences.Where(x => !existingResources.Contains(x.DBPediaResource));

        await dbContext.Preferences.AddRangeAsync(preferencesToAdd);

        // Slight risk here on race conditions with adding duplicated Preference
        // this will cause error but can live with that.
        // Should be rare and can be handled by upper level retry.
        await dbContext.SaveChangesAsync();

        return existingPreferences.Concat(preferencesToAdd);
    }

    // TODO: Move to new PreferencesExtractService
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
                var dbPediaResource = line.Trim().ToLower();

                preferences.Add(
                    new Preference()
                    {
                        DBPediaResource = dbPediaResource,
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
