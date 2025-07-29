using Socializer.Database.Models;

namespace Socializer.Chat.Interfaces;

public interface IChatMessageService
{
    Task<ChatMessage> AddMessageAsync(Guid userId, string chatHash, string message);
}
