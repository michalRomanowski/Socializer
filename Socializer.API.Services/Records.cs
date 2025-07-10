using Socializer.Database.Models;

namespace Socializer.API.Services;

public record UserMatch(User User1, User User2, IEnumerable<PreferenceMatch> PreferenceMatches, float MatchWeight);
public record PreferenceMatch(UserPreference User1Preference, UserPreference User2Preference, float MatchWeight);
