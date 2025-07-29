using Microsoft.EntityFrameworkCore;

namespace Socializer.Database.Models;

[Index(nameof(ChatHash), IsUnique = true)]
public class Chat : Entity
{
    public string ChatHash { get; set; }

    public List<ChatUser> ChatUsers { get; set; }

    public List<ChatMessage> Messages { get; set; }
}