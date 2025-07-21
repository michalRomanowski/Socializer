namespace Socializer.Chat.Interfaces;

public interface IChatService
{
    Task<Database.Models.Chat> AddChatAsync(IEnumerable<Guid> userIds, string connectionId);
}
