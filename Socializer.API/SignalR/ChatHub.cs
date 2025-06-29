using Microsoft.AspNetCore.SignalR;
using Socializer.API.Services.Interfaces;
using Socializer.LLM;

namespace Socializer.API.SignalR;

public class ChatHub(ILLMClient lLMClient, IPreferenceService preferenceService, ILogger<ChatHub> logger) : Hub
{
    public async Task SendMessage(string user, string message)
    {
        logger.LogDebug("Received message: '{Message}' from User: '{User}'.", message, user);

        await Clients.All.SendAsync("ReceiveMessage", user, message);

        var llmResponse = await lLMClient.QueryAsync(message);

        await Task.Delay(10000); // TODO: Delay added because of requests limit for free models

        var preferences = await preferenceService.GetPreferencesAsync(message);

        await Clients.All.SendAsync("ReceiveMessage", "bot", llmResponse);

        // TODO: for debug purposes, add config feature flag
        await Clients.All.SendAsync("ReceiveMessage", "bot", preferences.Select(x => $"{x.PreferenceType} {x.Url}"));
    }
}