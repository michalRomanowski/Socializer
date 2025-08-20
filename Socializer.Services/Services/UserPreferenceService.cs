using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.Services.Interfaces;

namespace Socializer.Services.Services;

internal class UserPreferenceService(
    SocializerDbContext dbContext,
    IPreferenceService preferenceService,
    ILogger<UserPreferenceService> logger) : IUserPreferenceService
{
    public async Task<IEnumerable<UserPreference>> GetAsync(Guid userId)
    {
        logger.LogDebug("Getting preferences for user {userId}", userId);

        var userPreferences = await dbContext
            .UserPreferences.Include(x => x.Preference)
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

        return userPreferences;
    }

    public async Task<IEnumerable<UserPreference>> AddOrUpdateAsync(Guid userId, IEnumerable<Preference> preferences)
    {
        logger.LogDebug("Adding {preferencesCount} preferences to user {userId}", preferences.Count(), userId);

        var preferencesAddedToDb = await preferenceService.GetOrAddAsync(preferences);

        return await AddOrUpdatePreferencesToUserAsync(userId, preferencesAddedToDb);
    }

    /// <param name="preferences">All preferences must already be present in db</param>
    private async Task<IEnumerable<UserPreference>> AddOrUpdatePreferencesToUserAsync(Guid userId, IEnumerable<Preference> preferences)
    {
        var preferencesIds = preferences.Select(x => x.Id).ToHashSet();

        var existingUserPreferences = await dbContext.UserPreferences
            .Where(up => up.UserId == userId && preferencesIds.Contains(up.PreferenceId))
            .ToListAsync();

        logger.LogDebug("User already has {existingUserPreferencesCount} preferences. Increasing count.", existingUserPreferences.Count);

        foreach (var eup in existingUserPreferences)
        {
            eup.Count++;
        }

        var preferencesToAddToUser = preferences.Where(p => !existingUserPreferences.Any(eup => eup.PreferenceId == p.Id));

        logger.LogDebug("Adding {preferencesToAddToUserCount} new user preference.", preferencesToAddToUser.Count());

        var addedUserPreferences = new HashSet<UserPreference>();

        foreach (var np in preferencesToAddToUser)
        {
            var newUserPreference = new UserPreference()
            {
                PreferenceId = np.Id,
                UserId = userId,
                Count = 1,
                Weight = 1.0f
            };

            addedUserPreferences.Add(newUserPreference);
            dbContext.UserPreferences.Add(newUserPreference);
        }

        // There is risk of UserPreferences corruption as there read and update db not wrapped in transaction.
        // Chance is small and it is acceptable to miss increase of UserPreference therefore performance > consistency in this case.
        await dbContext.SaveChangesAsync();

        return existingUserPreferences.Concat(addedUserPreferences);
    }
}
