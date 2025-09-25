using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.Services.Interfaces;

namespace Socializer.Services.Services;

internal class PreferenceService(SocializerDbContext dbContext, ILogger<PreferenceService> logger) : IPreferenceService
{
    public async Task<IEnumerable<Preference>> GetOrAddAsync(IEnumerable<Preference> preferences)
    {
        logger.LogDebug("Getting or adding {preferencesCount} preferences", preferences.Count());

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
}
