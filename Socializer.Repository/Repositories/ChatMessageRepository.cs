using Azure.Data.Tables;
using Socializer.Database.Models;
using Socializer.Database.NoSql.Models;
using Socializer.Repository.Interfaces;

namespace Socializer.Repository.Repositories;

internal class ChatMessageRepository(TableServiceClient serviceClient) : IChatMessageRepository
{
    public async Task AddAsync(string chatHash, ChatMessage chatMessage)
    {
        var tableClient = serviceClient.GetTableClient("Chats");

        var chatMessageEntity = new ChatMessageEntity(chatHash, chatMessage.Id, chatMessage.SenderId, chatMessage.Message);

        await tableClient.AddEntityAsync(chatMessageEntity);
    }
}
