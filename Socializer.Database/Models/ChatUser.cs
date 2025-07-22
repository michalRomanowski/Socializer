namespace Socializer.Database.Models;

public class ChatUser : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; }
}
