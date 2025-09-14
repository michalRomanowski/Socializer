using Azure.Data.Tables;
using Socializer.Database.NoSql.Models;
using Socializer.Repository.Interfaces;

namespace Socializer.Repository.Repositories;

internal class ChatMessageRepository(TableServiceClient serviceClient) : IChatMessageRepository
{
    private TableClient TableClient => serviceClient.GetTableClient("Chats");

    public async Task AddChatMessageAsync(Guid senderId, string chatHash, string message)
    {
        var chatMessageEntity = new ChatMessageEntity(chatHash, Guid.NewGuid(), senderId, message);

        await TableClient.AddEntityAsync(chatMessageEntity);
    }

    public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string chatHash)
    {
        // TODO: Order by Timestamp in DB and Pagination
        var queryResults = TableClient.QueryAsync<ChatMessageEntity>(entity => entity.PartitionKey == chatHash);
        var chatMessages = new List<ChatMessage>();

        await foreach (var chatMessageEntity in queryResults)
        {
            chatMessages.Add(
                new ChatMessage(
                    chatMessageEntity.SenderId,
                    chatMessageEntity.Message,
                    chatMessageEntity.Timestamp));
        }

        return chatMessages.OrderBy(cm => cm.Timestamp);
    }
}
