using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Chat.Extensions;
using Socializer.Chat.Interfaces;
using Socializer.LLM;
using System.Text;

namespace Socializer.Chat;

// TODO: This class has to be reconsidered due to many responsibilities. Split into some extra services probably.
[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub(
    ILLMClient lLMClient, 
    IPreferenceService preferenceService, 
    IUserPreferenceService userPreferenceService, 
    IUserService userService, 
    IChatService chatService,
    ICommandsService commandsService,
    IChatMessageService chatMessageService,
    ILogger<ChatHub> logger) : Hub
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

            await chatService.AddChatAsync([userResult.Result.Id], Context.ConnectionId);

            await Clients.All.SendAsync("ReceiveMessage", "bot", Messages.HelloMessage(userResult.Result.Username).ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in chat connection initialization. ConnectionId: {connectionId}.", Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMessage", "error", "Error initializing chat connection. Sorry.");
        }
    }

    // TODO: Should I send only user Id instead of unsername? Possibly yes for security reasons
    // Actualy Maybe I can read from identity like in controller, that would be safest.
    public async Task SendMessage(Guid userId, string username, string message)
    {
        try
        {
            if (message.Length > 800)
                message = message[..800]; // TODO: Chars limit configurable

            logger.LogDebug("Received message: '{Message}' from User: '{User}', ConnectionId: {connectionId}.", message, username, Context.ConnectionId);

            if (await commandsService.HandleCommandAsync(userId, message, Clients.All, Context.ConnectionId))
                return;

            await Clients.All.SendAsync("ReceiveMessage", username, message);

            await RespondToMessage(message);

            // TODO: Delay added because of requests limit for free models, generally updating preferences trigger will be different,
            // no need to call it for each message (unless I want to use it for conversation context).
            await Task.Delay(10000);

            await UpdatePreferences(username, message);

            await chatMessageService.AddMessageAsync(userId, message, Context.ConnectionId);
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

        await Clients.All.SendAsync("ReceiveMessage", "bot", preferences.ToMessage());
    }
}