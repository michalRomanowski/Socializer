using Socializer.Shared.Dtos;

namespace Socializer.Services.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ChatDto>> GetChatsAsync(Guid userId);

    Task<Database.Models.Chat> GetOrAddChatAsync(Guid userId);

    Task<Database.Models.Chat> GetOrAddChatAsync(string chatHash);

    Task<Database.Models.Chat> GetOrAddChatAsync(ISet<Guid> userIds);
}
