using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Socializer.API.Services;
using Socializer.API.Services.Interfaces;
using Socializer.LLM;
using System.Text;

namespace Socializer.API.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub(ILLMClient lLMClient, IPreferenceService preferenceService, IUserPreferenceService userPreferenceService, 
    IUserService userService, IUserMatchingService userMatchingService, ILogger<ChatHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        // TODO: Add some more error handling, maybe by sending message in chat and logging.
        var userResult = await userService.GetUserAsync(new Guid(Context.UserIdentifier));

        await Clients.All.SendAsync("ReceiveMessage", "bot", Messages.HelloMessage(userResult.Result.Username).ToString());
    }

    public async Task SendMessage(string username, string message)
    {
        try
        {
            if(message.Length > 800)
                message = message[..800]; // TODO: Chars limit configurable

            logger.LogDebug("Received message: '{Message}' from User: '{User}'.", message, username);

            if("p".Equals(message) || "preferences".Equals(message))
            {
                await UserPreferencesMessageAsync(username);
                return;
            }

            if ("m".Equals(message) || "matches".Equals(message))
            {
                var matches = await userMatchingService.UserMatchesAsync(username);

                await UserMatchesMessageAsync(matches);
                return;
            }

            await Clients.All.SendAsync("ReceiveMessage", username, message);

            var llmResponse = await lLMClient.QueryAsync(
                new StringBuilder(message)
                    .AppendHelpFindMoreInterests()
                    .AppendSameLanguageResponse(),
                200); // TODO: Limit can be configurable

            await Clients.All.SendAsync("ReceiveMessage", "bot", llmResponse);

            await Task.Delay(10000); // TODO: Delay added because of requests limit for free models

            var preferences = await preferenceService.ExtractPreferencesAsync(message);

            foreach(var p in preferences)
            {
                await userPreferenceService.UpdateOrAddAsync(username, p);
            }

            var newPreferencesMessage = string.Join("\r\n", preferences.Select(x => $"{x.PreferenceType} {x.DBPediaResource}"));

            // TODO: for debug purposes, add config feature flag
            await Clients.All.SendAsync("ReceiveMessage", "bot", newPreferencesMessage);
        }
        catch (Exception ex){
            logger.LogError(ex, "Exception in chat.");
        }
    }

    private async Task UserPreferencesMessageAsync(string username)
    {
        var userPreferences = await userPreferenceService.GetAsync(username);
        var userPreferencesMessage = string.Join("\r\n", userPreferences.Select(x => $"{x.Preference.DBPediaResource} {x.Preference.PreferenceType} {x.Count} {x.Weight}"));

        await Clients.All.SendAsync("ReceiveMessage", "bot", userPreferencesMessage);
    }

    private async Task UserMatchesMessageAsync(IEnumerable<UserMatch> matches)
    {
        var sb = new StringBuilder();

        foreach (var m in matches.OrderByDescending(x => x.MatchWeight))
        {
            sb.AppendLine($"{m.User2.Username} {m.MatchWeight}");

            foreach(var pm in m.PreferenceMatches.OrderByDescending(x => x.MatchWeight))
            {
                sb.AppendLine($"- {pm.User1Preference.Preference.DBPediaResource} {pm.MatchWeight}");
            }

            sb.AppendLine();
        }

        await Clients.All.SendAsync("ReceiveMessage", "bot", sb.ToString());
    }
}