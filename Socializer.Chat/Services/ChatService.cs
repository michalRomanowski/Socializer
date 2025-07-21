using Microsoft.Extensions.Logging;
using Socializer.Chat.Interfaces;
using Socializer.Database;

namespace Socializer.Chat.Services;

internal class ChatService(SocializerDbContext dbContext, ILogger<ChatService> logger) : IChatService
{
    public async Task<Database.Models.Chat> AddChatAsync(IEnumerable<Guid> userIds, string connectionId)
    {
        logger.LogDebug("Saving chat in db, ConnectionId: {connectionId}.", connectionId);

        var chat = new Database.Models.Chat()
        {
            UserIds = [.. userIds],
            ConnectionId = connectionId
        };

        await dbContext.Chats.AddAsync(chat);
        await dbContext.SaveChangesAsync();

        return chat;
    }
}
