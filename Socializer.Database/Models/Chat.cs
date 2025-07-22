using Microsoft.EntityFrameworkCore;

namespace Socializer.Database.Models;

[Index(nameof(ConnectionId), IsUnique = true)]
public class Chat : Entity
{
    public string ConnectionId { get; set; }
    public List<ChatUser> ChatUsers { get; set; }
    //public Guid ChatContinuedFrom { get; set; } // TODO: Keep here to remember idea, might do it or remove later
    //public Guid ChatContinuedIn { get; set; }

    public List<ChatMessage> Messages { get; set; }
}