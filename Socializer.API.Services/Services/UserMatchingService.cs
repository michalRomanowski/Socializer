using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Microsoft.EntityFrameworkCore;

namespace Socializer.API.Services.Services;

public class UserMatchingService(SocializerDbContext dbContext) : IUserMatchingService
{
    public async Task<IEnumerable<UserMatch>> UserMatchesAsync(string username)
    {
        var user = await dbContext.Users
            .Include(x => x.UserPreferences)
            .ThenInclude(x => x.Preference)
            .AsNoTracking()
            .SingleAsync(x => x.Username == username);

        var preferenceMatches = new List<PreferenceMatch>();

        foreach (var up1 in user.UserPreferences)
        {
            var otherUsersSharedPreferences = await dbContext.UserPreferences
                .Include(x => x.User)
                .Where(x => x.PreferenceId == up1.PreferenceId && x.UserId != user.Id)
                .AsNoTracking()
                .ToListAsync();

            foreach (var up2 in otherUsersSharedPreferences)
            {
                var preferenceMatch = new PreferenceMatch(up1, up2, up1.Count * up2.Count * up1.Weight * up2.Weight);

                preferenceMatches.Add(preferenceMatch);
            }
        }

        var userPreferencesMatches = preferenceMatches
            .GroupBy(x => x.User2Preference.User.Id)
            .Select(x => new UserMatch(user, x.First().User2Preference.User, x, x.Sum(x => x.MatchWeight)));

        return userPreferencesMatches;
    }
}
