using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Socializer.API.Services.Interfaces;
using Socializer.Chat.Extensions;
using Socializer.Chat.Interfaces;

namespace Socializer.Chat.Services;

internal class CommandsService(
    IUserPreferenceService userPreferenceService,
    IUserMatchingService userMatchingService,
    ILogger<CommandsService> logger) : ICommandsService
{
    public async Task<bool> HandleCommandAsync(Guid userId, string message, IClientProxy clientProxy, string connectionId)
    {
        switch (message.ToLowerInvariant())
        {
            case "p":
                logger.LogDebug("Preferences command message, ConnectionId: {connectionId}.", connectionId);

                var userPreferences = await userPreferenceService.GetAsync(userId);

                // TODO: Add extension to clientProxy or sending messages service to avoid repetition of "ReceiveMessage", "bot"
                await clientProxy.SendAsync("ReceiveMessage", "bot", userPreferences.ToMessage());

                return true;

            case "m":
                logger.LogDebug("Matches command message, ConnectionId: {connectionId}.", connectionId);
                var matches = await userMatchingService.UserMatchesAsync(userId);
                await clientProxy.SendAsync("ReceiveMessage", "bot", matches.ToMessage());
                return true;

            default:
                logger.LogDebug("Non command message, ConnectionId: {connectionId}.", connectionId);
                return false;
        }
    }
}
