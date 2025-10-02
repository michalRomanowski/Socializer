using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.Services.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ChatDto>> GetChatsAsync(Guid userId);

    Task<Chat> GetOrAddChatAsync(Guid userId);

    Task<Chat> GetOrAddChatAsync(string chatHash);

    Task<Chat> GetOrAddChatAsync(ISet<Guid> userIds);

    Task DeleteChatAsync(Guid userId, Guid chatId);
}
