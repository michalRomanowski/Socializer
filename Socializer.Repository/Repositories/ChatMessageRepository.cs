using Azure.Data.Tables;
using Socializer.Database.NoSql.Models;
using Socializer.Repository.Interfaces;

namespace Socializer.Repository.Repositories;

internal class ChatMessageRepository(TableServiceClient serviceClient) : IChatMessageRepository
{
    public async Task AddChatMessageAsync(Guid senderId, string chatHash, string message)
    {
        var tableClient = serviceClient.GetTableClient("Chats");

        var chatMessageEntity = new ChatMessageEntity(chatHash, Guid.NewGuid(), senderId, message);

        await tableClient.AddEntityAsync(chatMessageEntity);
    }
}
