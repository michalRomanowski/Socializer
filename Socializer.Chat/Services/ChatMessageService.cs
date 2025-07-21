using Microsoft.EntityFrameworkCore;
using Socializer.Chat.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;

namespace Socializer.Chat.Services;

internal class ChatMessageService(SocializerDbContext dbContext) : IChatMessageService
{
    public async Task<ChatMessage> AddMessageAsync(Guid userId, string message, string connectionId)
    {
        var chat = await dbContext.Chats.SingleAsync(x => x.ConnectionId.Equals(connectionId));

        var chatMessage = new ChatMessage()
        {
            ChatId = chat.Id,
            SenderId = userId,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        await dbContext.ChatMessages.AddAsync(chatMessage);
        await dbContext.SaveChangesAsync();

        return chatMessage;
    }
}
