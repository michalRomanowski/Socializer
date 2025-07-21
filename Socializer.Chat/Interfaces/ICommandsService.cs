using Microsoft.AspNetCore.SignalR;

namespace Socializer.Chat.Interfaces;

public interface ICommandsService
{
    /// <returns>True if message invoked command handling. False if message unrecognized and did nothing.</returns>
    Task<bool> HandleCommandAsync(Guid userId, string message, IClientProxy clientProxy, string connectionId);
}
