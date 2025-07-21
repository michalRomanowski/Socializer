using Socializer.API.Services.Interfaces;
using Socializer.Database;
using Microsoft.EntityFrameworkCore;
using Common.Utils;
using Socializer.Shared.Dtos;
using AutoMapper;

namespace Socializer.API.Services.Services;

internal class UserMatchingService(SocializerDbContext dbContext, IMapper mapper) : IUserMatchingService
{
    public async Task<OperationResult<IEnumerable<UserMatchDto>>> UserMatchesAsync(Guid userId)
    {
        var user = await dbContext.Users
            .Include(x => x.UserPreferences)
            .ThenInclude(x => x.Preference)
            .AsNoTracking()
            .SingleAsync(x => x.Id == userId);

        return await UserMatchesAsync(user);
    }

    private async Task<OperationResult<IEnumerable<UserMatchDto>>> UserMatchesAsync(Database.Models.User user)
    {
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

        var mappedUserPreferencesMatches = mapper.Map<IEnumerable<UserMatchDto>>(userPreferencesMatches);

        return OperationResult<IEnumerable<UserMatchDto>>.Success(mappedUserPreferencesMatches);
    }
}
