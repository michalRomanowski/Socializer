using Microsoft.AspNetCore.SignalR;
using Socializer.API.Services.Interfaces;
using Socializer.LLM;

namespace Socializer.API.SignalR;

public class ChatHub(ILLMClient lLMClient, IPreferenceService preferenceService, ILogger<ChatHub> logger) : Hub
{
    public async Task SendMessage(string user, string message)
    {
        try
        {
            logger.LogDebug("Received message: '{Message}' from User: '{User}'.", message, user);

            await Clients.All.SendAsync("ReceiveMessage", user, message);

            var llmResponse = await lLMClient.QueryAsync(message, 200); // TODO: Limit can be configurable
            await Clients.All.SendAsync("ReceiveMessage", "bot", llmResponse);

            await Task.Delay(10000); // TODO: Delay added because of requests limit for free models

            var preferences = await preferenceService.GetPreferencesAsync(message);
            var preferencesMessage = string.Join("\r\n", preferences.Select(x => $"{x.PreferenceType} {x.Url}"));

            // TODO: for debug purposes, add config feature flag
            await Clients.All.SendAsync("ReceiveMessage", "bot", preferencesMessage);
        }
        catch (Exception ex){
            logger.LogError(ex, "Exception in chat.");
        }
    }
}