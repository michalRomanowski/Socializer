namespace Socializer.Repository.Interfaces;

/// <summary>
/// Hides NoSql database for storing Chats
/// </summary>
public interface IChatRepository
{
    Task DeleteChatAsync(string chatHash);
}