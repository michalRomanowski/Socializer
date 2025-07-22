using Socializer.Shared.Dtos;

namespace Socializer.Chat.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ChatDto>> GetChatsAsync(Guid userId);

    Task<Database.Models.Chat> AddChatAsync(IEnumerable<Guid> userIds, string connectionId);
}
