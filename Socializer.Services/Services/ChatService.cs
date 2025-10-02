using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Socializer.Database;
using Socializer.Database.Models;
using Socializer.Repository.Interfaces;
using Socializer.Services.Interfaces;
using Socializer.Shared.Dtos;
using Socializer.Shared.Extensions;

namespace Socializer.Services.Services;

public class ChatService(SocializerDbContext dbContext, IChatRepository chatRepository, ILogger<ChatService> logger) : IChatService
{
    public async Task<IEnumerable<ChatDto>> GetChatsAsync(Guid userId)
    {
        logger.LogDebug("Getting chats for user {userId}.", userId);

        var chatDtos = await dbContext.ChatUsers
            .Where(cu => cu.UserId == userId)
            .Select(cu => cu.Chat)
            .Select(c => new ChatDto
            {
                Id = c.Id,
                ChatHash = c.ChatHash,
                Usernames = c.ChatUsers.Select(cu => cu.User.Username)
            })
            .AsNoTracking()
            .ToListAsync();

        return chatDtos;
    }

    public async Task<Chat> GetOrAddChatAsync(Guid userId)
    {
        return await GetOrAddChatAsync(userId.GenerateChatHash(), new HashSet<Guid>() { userId });
    }

    public async Task<Chat> GetOrAddChatAsync(string chatHash)
    {
        return await GetOrAddChatAsync(chatHash, chatHash.SplitChatHash());
    }

    public async Task<Chat> GetOrAddChatAsync(ISet<Guid> userIds)
    {
        return await GetOrAddChatAsync(userIds.GenerateChatHash(), userIds);
    }

    private async Task<Chat> GetOrAddChatAsync(string chatHash, ISet<Guid> userIds)
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

        var chat = new Chat()
        {
            ChatUsers = chatUsers,
            ChatHash = chatHash
        };

        await dbContext.Chats.AddAsync(chat);
        await dbContext.SaveChangesAsync();

        return chat;
    }

    public async Task DeleteChatAsync(Guid userId, Guid chatId)
    {
        logger.LogDebug("Deleting chat {chatId} for user {userId}", chatId, userId);

        // Execution strategy necessary for manual transactionas as sql configured for retries
        // it relies on transactions already which causes conflict when manually trying to use one here
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {

            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                var chat = await dbContext.Chats
                    .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId) && c.Id == chatId)
                    .SingleAsync();

                dbContext.Remove(chat);
                await dbContext.SaveChangesAsync();

                await chatRepository.DeleteChatAsync(chat.ChatHash);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting chat {chatId} for user {userId}. Rolling back transaction.", chatId, userId);
                transaction.Rollback();
            }
        });
    }
}
