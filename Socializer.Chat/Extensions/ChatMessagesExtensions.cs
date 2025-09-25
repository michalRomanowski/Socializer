using Socializer.Database.Models;
using Socializer.Shared.Dtos;
using System.Text;

namespace Socializer.Chat.Extensions;

internal static class ChatMessagesExtensions
{
    public static string ToMessage(this IEnumerable<Preference> preferences)
    {
        var message = string.Join("\r\n", preferences.Select(x => $"{x.DBPediaResource}"));
        return message;
    }

    public static string ToMessage(this IEnumerable<UserPreferenceDto> userPreferences)
    {
        var message = string.Join("\r\n", userPreferences.Select(x => $"{x.DBPediaResource} {x.Count} {x.Weight}"));
        return message;
    }

    public static string ToMessage(this IEnumerable<UserMatchDto> matches)
    {
        var sbMessage = new StringBuilder();

        foreach (var m in matches.OrderByDescending(x => x.MatchWeight))
        {
            sbMessage.AppendLine($"{m.User2Name} {m.MatchWeight}");

            foreach (var pm in m.PreferenceMatches.OrderByDescending(x => x.MatchWeight))
            {
                sbMessage.AppendLine($"- {pm.PreferenceName} {pm.MatchWeight}");
            }

            sbMessage.AppendLine();
        }

        return sbMessage.ToString();
    }
}
