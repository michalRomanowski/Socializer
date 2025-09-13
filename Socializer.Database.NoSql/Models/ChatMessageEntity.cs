using Azure;
using Azure.Data.Tables;

namespace Socializer.Database.NoSql.Models;

public class ChatMessageEntity : ITableEntity
{
    public string PartitionKey { get; set; } // ChatHash
    public string RowKey { get; set; } // MessageId

    // Properties
    public Guid SenderId { get; set; }
    public string Message { get; set; }


    // Required by ITableEntity
    public ETag ETag { get; set; } = ETag.All;
    public DateTimeOffset? Timestamp { get; set; }

    //public ChatMessageEntity() { }

    public ChatMessageEntity(string chatHash, Guid messageId, Guid senderId, string message)
    {
        PartitionKey = chatHash;
        RowKey = messageId.ToString(); // unique row id
        SenderId = senderId;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}