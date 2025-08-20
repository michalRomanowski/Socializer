using Socializer.Database.Models;

namespace Socializer.Services.Interfaces.Chat;

public interface IChatMessageService
{
    Task<ChatMessage> AddMessageAsync(Guid userId, string chatHash, string message);
}
