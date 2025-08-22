using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.Services.Interfaces.Chat;
using Socializer.Shared.Dtos;
using Socializer.Shared.Extensions;

namespace Socializer.Services.Services.Chat;

// TODO: For now accept crappy approach to chat hash as this will be re-created for sake of noSql approach in future anyway
public class ChatService(SocializerDbContext dbContext, ILogger<ChatService> logger) : IChatService
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

    public async Task<Database.Models.Chat> GetOrAddChatAsync(Guid userId)
    {
        return await GetOrAddChatAsync(userId.GenerateChatHash(), new HashSet<Guid>() { userId });
    }

    public async Task<Database.Models.Chat> GetOrAddChatAsync(string chatHash)
    {
        return await GetOrAddChatAsync(chatHash, chatHash.SplitChatHash());
    }

    public async Task<Database.Models.Chat> GetOrAddChatAsync(ISet<Guid> userIds)
    {
        return await GetOrAddChatAsync(userIds.GenerateChatHash(), userIds);
    }

    private async Task<Database.Models.Chat> GetOrAddChatAsync(string chatHash, ISet<Guid> userIds)
    {
        var existingChat = await dbContext.Chats.SingleOrDefaultAsync(x => x.ChatHash == chatHash);

        if (existingChat != default)
        {
            logger.LogDebug("Found chat in db with hash: {hash}.", chatHash);
            return existingChat;
        }

        logger.LogDebug("Not found chat in db with hash: {hash}. Creating new chat.", chatHash);

        var users = await dbContext.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();

        if (users.Count != userIds.Count)
        {
            var foundIds = users.Select(x => x.Id).ToHashSet();
            var notFoundIds = userIds.Where(x => !foundIds.Contains(x));

            throw new KeyNotFoundException($"Not found UserIds: {string.Join(' ', notFoundIds)}.");
        }

        var chatUsers = userIds.Select(x => new ChatUser() { UserId = x }).ToList();

        var chat = new Database.Models.Chat()
        {
            ChatUsers = chatUsers,
            ChatHash = chatHash
        };

        await dbContext.Chats.AddAsync(chat);
        await dbContext.SaveChangesAsync();

        return chat;
    }
}
