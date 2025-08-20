using Microsoft.EntityFrameworkCore;
using Socializer.API.Services.Interfaces.Chat;
using Socializer.Database;
using Socializer.Database.Models;

namespace Socializer.API.Services.Services.Chat;

public class ChatMessageService(SocializerDbContext dbContext) : IChatMessageService
{
    public async Task<ChatMessage> AddMessageAsync(Guid userId, string chatHash, string message)
    {
        var chat = await dbContext.Chats.SingleAsync(x => x.ChatHash.Equals(chatHash));

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
