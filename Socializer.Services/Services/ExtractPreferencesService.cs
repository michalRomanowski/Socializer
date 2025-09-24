using Microsoft.Extensions.Logging;
using Socializer.Database.Models;
using Socializer.LLM;
using Socializer.Services.Interfaces;

namespace Socializer.Services.Services;

internal class ExtractPreferencesService(ILLMClient llmClient, IReadPreferencesService preferencesReaderService, ILogger<ExtractPreferencesService> logger) : IExtractPreferencesService
{
    public async Task<IEnumerable<Preference>> ExtractPreferencesAsync(string message)
    {
        var preferencesPrompt = Prompts.PreferencesPrompt(message);

        logger.LogDebug("Preferences prompt: {PreferencesPrompt}", preferencesPrompt);

        var response = await llmClient.QueryAsync(preferencesPrompt, 700);

        var preferences = preferencesReaderService.ReadPreferences(response);

        logger.LogInformation("Extracted {PreferencesCount} preferences.", preferences.Count());

        return preferences;
    }
}
