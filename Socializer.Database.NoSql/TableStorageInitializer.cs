using Azure.Data.Tables;

namespace Socializer.Database.NoSql;

public class TableStorageInitializer(TableServiceClient serviceClient)
{
    public async Task CreateTablesIfNotExistAsync()
    {
        var chatsTableClient = serviceClient.GetTableClient("Chats");

        // Create the table if not exists
        await chatsTableClient.CreateIfNotExistsAsync();
    }
}
