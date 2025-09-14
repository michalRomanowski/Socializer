namespace Socializer.Repository.Interfaces;

public interface IChatMessageRepository
{
    Task AddChatMessageAsync(Guid senderId, string chatHash, string message);
}
