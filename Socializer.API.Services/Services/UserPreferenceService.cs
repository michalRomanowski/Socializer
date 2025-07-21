using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;

namespace Socializer.API.Services.Services;

public class UserPreferenceService(
    SocializerDbContext dbContext,
    IPreferenceService preferenceService,
    ILogger<UserPreferenceService> logger) : IUserPreferenceService
{
    public async Task<IEnumerable<UserPreference>> GetAsync(Guid userId)
    {
        logger.LogDebug("Getting preferences for user {userId}", userId);

        var userPreferences = await dbContext.UserPreferences.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();

        return userPreferences;
    }

    public async Task<IEnumerable<UserPreference>> AddOrUpdateAsync(string username, IEnumerable<Preference> preferences)
    {
        logger.LogDebug("Adding {preferencesCount} preferences to user {username}", preferences.Count(), username);

        var preferencesAddedToDb = await preferenceService.GetOrAddAsync(preferences);

        var user = await dbContext.Users.SingleAsync(x => x.Username == username);

        return await AddOrUpdateAsync(user, preferencesAddedToDb);
    }

    /// <summary>
    /// <param name="preferences">All preferences must already be present in db</param>
    private async Task<IEnumerable<UserPreference>> AddOrUpdateAsync(User user, IEnumerable<Preference> preferences)
    {
        var preferencesIds = preferences.Select(x => x.Id).ToHashSet();

        var existingUserPreferences = await dbContext.UserPreferences
            .Where(up => up.UserId == user.Id && preferencesIds.Contains(up.PreferenceId))
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
                UserId = user.Id,
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
