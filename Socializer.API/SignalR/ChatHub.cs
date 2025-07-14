using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Socializer.API.Services.Interfaces;
using Socializer.Database.Models;
using Socializer.LLM;
using Socializer.Shared.Dtos;
using System.Text;

namespace Socializer.API.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub(ILLMClient lLMClient, IPreferenceService preferenceService, IUserPreferenceService userPreferenceService, 
    IUserService userService, IUserMatchingService userMatchingService, ILogger<ChatHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        try
        {
            // TODO: Filter or other approach can be used for trackingId
            logger.LogDebug("Chat connection init,  ConnectionId: {connectionId}.", Context.ConnectionId);

            var userResult = await userService.GetUserAsync(new Guid(Context.UserIdentifier));

            if(!userResult.IsSuccess)
            {
                throw new Exception(userResult.ErrorMessage);
            }

            await Clients.All.SendAsync("ReceiveMessage", "bot", Messages.HelloMessage(userResult.Result.Username).ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in chat connection initialization. ConnectionId: {connectionId}.", Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMessage", "error", "Error initializing chat connection. Sorry.");
        }
    }

    public async Task SendMessage(string username, string message)
    {
        try
        {
            if (message.Length > 800)
                message = message[..800]; // TODO: Chars limit configurable

            logger.LogDebug("Received message: '{Message}' from User: '{User}', ConnectionId: {connectionId}.", message, username, Context.ConnectionId);

            if (await ChatCommands(username, message))
                return;

            await Clients.All.SendAsync("ReceiveMessage", username, message);

            await RespondToMessage(message);

            await Task.Delay(10000); // TODO: Delay added because of requests limit for free models

            await UpdatePreferences(username, message);
        }
        catch (Exception ex){
            logger.LogError(ex, "Exception in chat. ConnectionId: {connectionId}.", Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMessage", "error", "Error processing message. Sorry.");
        }
    }

    private async Task RespondToMessage(string message)
    {
        logger.LogDebug("Responding to message, ConnectionId: {connectionId}.", Context.ConnectionId);

        var llmResponse = await lLMClient.QueryAsync(
            new StringBuilder(message)
                .AppendHelpFindMoreInterests()
                .AppendSameLanguageResponse(),
            200); // TODO: Limit can be configurable

        await Clients.All.SendAsync("ReceiveMessage", "bot", llmResponse);
    }

    private async Task UpdatePreferences(string username, string message)
    {
        logger.LogDebug("Updating preferences, ConnectionId: {connectionId}.", Context.ConnectionId);

        var preferences = await preferenceService.ExtractPreferencesAsync(message);

        await userPreferenceService.AddOrUpdateAsync(username, preferences);

        await PreferencesMessageAsync(preferences);
    }

    private async Task<bool> ChatCommands(string username, string message)
    {
        switch (message.ToLowerInvariant())
        {
            case "p":
            case "P":
            case "preferences":
                logger.LogDebug("Preferences command message, ConnectionId: {connectionId}.", Context.ConnectionId);
                await UserPreferencesMessageAsync(username);
                return true;

            case "m":
            case "M":
            case "matches":
                logger.LogDebug("Matches command message, ConnectionId: {connectionId}.", Context.ConnectionId);
                var matches = await userMatchingService.UserMatchesAsync(username);
                await UserMatchesMessageAsync(matches.Result);
                return true;

            default:
                logger.LogDebug("Non command message, ConnectionId: {connectionId}.", Context.ConnectionId);
                return false;
        }
    }

    private async Task PreferencesMessageAsync(IEnumerable<Preference> preferences)
    {
        var newPreferencesMessage = string.Join("\r\n", preferences.Select(x => $"{x.DBPediaResource}"));
        await Clients.All.SendAsync("ReceiveMessage", "bot", newPreferencesMessage);
    }

    private async Task UserPreferencesMessageAsync(string username)
    {
        var userPreferences = await userPreferenceService.GetAsync(username);
        var userPreferencesMessage = string.Join("\r\n", userPreferences.Select(x => $"{x.Preference.DBPediaResource} {x.Count} {x.Weight}"));

        await Clients.All.SendAsync("ReceiveMessage", "bot", userPreferencesMessage);
    }

    private async Task UserMatchesMessageAsync(IEnumerable<UserMatchDto> matches)
    {
        var sb = new StringBuilder();

        foreach (var m in matches.OrderByDescending(x => x.MatchWeight))
        {
            sb.AppendLine($"{m.User2Name} {m.MatchWeight}");

            foreach(var pm in m.PreferenceMatches.OrderByDescending(x => x.MatchWeight))
            {
                sb.AppendLine($"- {pm.PreferenceName} {pm.MatchWeight}");
            }

            sb.AppendLine();
        }

        await Clients.All.SendAsync("ReceiveMessage", "bot", sb.ToString());
    }
}