using Azure.Data.Tables;
using Socializer.Database.NoSql.Models;

namespace Socializer.Database.NoSql;

public class TableStorageInitializer(TableServiceClient serviceClient)
{
    public async Task CreateTablesIfNotExistAsync()
    {
        var chatsTableClient = serviceClient.GetTableClient("Chats");

        // Create the table if not exists
        await chatsTableClient.CreateIfNotExistsAsync();

        //// Insert an entity
        var chatMessage = new ChatMessageEntity("tmp1", Guid.NewGuid(), Guid.NewGuid(), "msg");

        await chatsTableClient.AddEntityAsync(chatMessage);
    }
}
