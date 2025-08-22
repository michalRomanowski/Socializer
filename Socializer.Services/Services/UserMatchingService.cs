using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Socializer.Database;
using Socializer.Services.Interfaces;
using Socializer.Shared.Dtos;

namespace Socializer.Services.Services;

internal class UserMatchingService(SocializerDbContext dbContext, IMapper mapper) : IUserMatchingService
{
    public async Task<IEnumerable<UserMatchDto>> UserMatchesAsync(Guid userId)
    {
        var user = await dbContext.Users
            .Include(x => x.UserPreferences)
            .ThenInclude(x => x.Preference)
            .AsNoTracking()
            .SingleAsync(x => x.Id == userId);

        return await UserMatchesAsync(user);
    }

    private async Task<IEnumerable<UserMatchDto>> UserMatchesAsync(Database.Models.User user)
    {
        var preferenceMatches = new List<PreferenceMatch>();

        foreach (var up1 in user.UserPreferences)
        {
            // This will have to be optimized for more users
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

        var mappedUserPreferencesMatches = mapper.Map<IEnumerable<UserMatchDto>>(userPreferencesMatches);

        return mappedUserPreferencesMatches;
    }
}
