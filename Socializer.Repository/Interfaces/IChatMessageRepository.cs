using Socializer.Database.Models;

namespace Socializer.Repository.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(string chatHash, ChatMessage chatMessage);
}
