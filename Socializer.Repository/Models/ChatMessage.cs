namespace Socializer.Repository.Models;

public record ChatMessage(Guid SenderId, string Message, DateTimeOffset? Timestamp);
