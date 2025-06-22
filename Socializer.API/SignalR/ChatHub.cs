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
    }
}