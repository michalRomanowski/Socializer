namespace Socializer.Shared.Dtos;

public record UserMatchDto(Guid User1Id, Guid User2Id, string User1Name, string User2Name, IEnumerable<PreferenceMatchDto> PreferenceMatches, float MatchWeight);
public record PreferenceMatchDto(string PreferenceName, float MatchWeight);