namespace Socializer.Repository.Interfaces;

/// <summary>
/// Hides NoSql database for storing Chat Messages
/// </summary>
public interface IChatMessageRepository
{
    Task AddChatMessageAsync(Guid senderId, string chatHash, string message);
}
