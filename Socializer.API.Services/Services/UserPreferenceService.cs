using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;

namespace Socializer.API.Services.Services;

public class UserPreferenceService(SocializerDbContext dbContext, IPreferenceService preferenceService, ILogger<UserPreferenceService> logger) : IUserPreferenceService
{
    public async Task<IEnumerable<UserPreference>> GetAsync(string username)
    {
        logger.LogDebug("Getting preferences for user {username}", username);

        var user = await dbContext.Users
            .Include(x => x.UserPreferences)
            .ThenInclude(x => x.Preference)
            .SingleAsync(x => x.Username == username);

        return user.UserPreferences.OrderByDescending(x => x.Count);
    }

    public async Task<UserPreference> UpdateOrAddAsync(string username, Preference preference)
    {
        logger.LogDebug("Adding preference {preference} to user {username}", preference.DBPediaResource, username);

        preference = await preferenceService.GetOrAddAsync(preference);

        var user = await dbContext.Users.SingleAsync(x => x.Username == username);

        var existingUserPreference =
            await dbContext.UserPreferences.SingleOrDefaultAsync(x => x.UserId == user.Id && x.PreferenceId == preference.Id);

        if (existingUserPreference != null)
        {
            existingUserPreference.Count++;
            logger.LogDebug("UserPreference exists. Updating count to {userPreferenceCount}.", existingUserPreference.Count);
        }
        else
        {
            logger.LogDebug("Creating new UserPreference.");

            existingUserPreference = new UserPreference()
            {
                Preference = preference,
                User = user,
                Count = 1,
                Weight = 1.0f
            };

            dbContext.Add(existingUserPreference);
        }

        await dbContext.SaveChangesAsync();

        return existingUserPreference;
    }
}
