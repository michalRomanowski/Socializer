using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.Chat.Interfaces;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.Chat.Services;

internal class ChatService(SocializerDbContext dbContext, ILogger<ChatService> logger) : IChatService
{
    public async Task<IEnumerable<ChatDto>> GetChatsAsync(Guid userId)
    {
        logger.LogDebug("Getting chats for user {userId}.", userId);

        var chatUsers = await dbContext.ChatUsers
            .Where(cu => cu.UserId == userId)
            .GroupBy(cu => cu.ChatId)
            .Select(cug => new { ChatId = cug.Key, Usernames = cug.Select(x => x.User.Username) })
            .ToListAsync();

        return chatUsers.Select(x => new ChatDto() { Id = x.ChatId, Usernames = [.. x.Usernames] });
    }

    public async Task<Database.Models.Chat> AddChatAsync(IEnumerable<Guid> userIds, string connectionId)
    {
        logger.LogDebug("Saving chat in db, ConnectionId: {connectionId}.", connectionId);

        var users = await dbContext.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();

        var chatUsers = userIds.Select(x => new ChatUser() { UserId = x }).ToList();

        var chat = new Database.Models.Chat()
        {
            ChatUsers = chatUsers,
            ConnectionId = connectionId
        };

        await dbContext.Chats.AddAsync(chat);
        await dbContext.SaveChangesAsync();

        return chat;
    }
}
