namespace Socializer.Repository.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(Guid senderId, string chatHash, string message);
}
