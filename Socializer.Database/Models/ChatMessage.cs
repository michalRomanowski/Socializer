using Microsoft.EntityFrameworkCore;

namespace Socializer.Database.Models
{
    [Index(nameof(Timestamp))]
    public class ChatMessage : Entity
    {
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }

        public Guid SenderId { get; set; }
        public User Sender { get; set; }

        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
