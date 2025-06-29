using Microsoft.AspNetCore.SignalR;
using Socializer.LLM;

namespace Socializer.API.SignalR;

public class ChatHub(ILLMClient lLMClient) : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);

        var llmResponse = await lLMClient.QueryAsync(message);

        await Clients.All.SendAsync("ReceiveMessage", "bot", llmResponse);

        await Task.Delay(5000); // TODO: Delay added because of requests limit for free models

        var preferences = await lLMClient.GetPreferences(message);
    }
}