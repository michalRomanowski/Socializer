using Azure.Data.Tables;
using Socializer.Repository.Interfaces;

namespace Socializer.Repository.Repositories;

internal class ChatRepository(TableServiceClient serviceClient) : IChatRepository
{
    private TableClient TableClient => serviceClient.GetTableClient("Chats");

    public async Task DeleteChatAsync(string chatHash)
    {
        // Inefficient approach but good for now. It will rarely occure
        var chatMessages = TableClient.QueryAsync<TableEntity>(e => e.PartitionKey == chatHash);

        var batch = new List<TableTransactionAction>();

        await foreach (var message in chatMessages)
        {
            batch.Add(new TableTransactionAction(TableTransactionActionType.Delete, message));

            // Submit in chunks of 100 (Azure Table limit per batch)
            if (batch.Count == 100)
            {
                await TableClient.SubmitTransactionAsync(batch);
                batch.Clear();
            }
        }

        // Submit any remaining items
        if (batch.Count > 0)
        {
            await TableClient.SubmitTransactionAsync(batch);
        }
    }
}
