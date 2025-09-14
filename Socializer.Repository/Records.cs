namespace Socializer.Repository;

public record ChatMessage(Guid SenderId, string Message, DateTimeOffset? Timestamp);
