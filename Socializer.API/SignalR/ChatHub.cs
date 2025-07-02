using Microsoft.AspNetCore.SignalR;
using Socializer.API.Services.Interfaces;
using Socializer.LLM;
using System.Text;

namespace Socializer.API.SignalR;

public class ChatHub(ILLMClient lLMClient, IPreferenceService preferenceService, IUserService userService, ILogger<ChatHub> logger) : Hub
{
    public async Task SendMessage(string username, string message)
    {
        try
        {
            logger.LogDebug("Received message: '{Message}' from User: '{User}'.", message, username);

            await Clients.All.SendAsync("ReceiveMessage", username, message);

            var llmResponse = await lLMClient.QueryAsync(new StringBuilder(message), 200); // TODO: Limit can be configurable
            await Clients.All.SendAsync("ReceiveMessage", "bot", llmResponse);

            await Task.Delay(10000); // TODO: Delay added because of requests limit for free models

            var preferences = await preferenceService.GetPreferencesAsync(message);
            var updatedUser = await userService.AddPreferencesAsync(username, preferences);

            var newPreferencesMessage = string.Join("\r\n", preferences.Select(x => $"{x.PreferenceType} {x.Url}"));
            var allPreferencesMessage = string.Join("\r\n", updatedUser.Preferences.Select(x => $"{x.PreferenceType} {x.Url}"));

            // TODO: for debug purposes, add config feature flag
            await Clients.All.SendAsync("ReceiveMessage", "bot", newPreferencesMessage);
            await Clients.All.SendAsync("ReceiveMessage", "bot", allPreferencesMessage);
        }
        catch (Exception ex){
            logger.LogError(ex, "Exception in chat.");
        }
    }
}