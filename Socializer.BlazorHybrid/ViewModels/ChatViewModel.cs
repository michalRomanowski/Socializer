namespace Socializer.BlazorHybrid.ViewModels
{
    internal class ChatMessage
    {
        public string Author { get; set; }
        public string Content { get; set; }
    }

    internal class ChatViewModel
    {
        public string NewMessage { get; set; } = string.Empty;

        public List<ChatMessage> Messages { get; set; } = [
            new ChatMessage(){ Author = "Bot", Content = "Message1"},
            new ChatMessage(){ Author = "user2", Content = "Message2"},
            new ChatMessage(){ Author = "Michal", Content = "Message3"},
            new ChatMessage(){ Author = "user3", Content = "Message4"},
            new ChatMessage(){ Author = "Bot", Content = "Message5"},
            new ChatMessage(){ Author = "user1", Content = "Message6"},
            new ChatMessage(){ Author = "Michal", Content = "Message7"},
            new ChatMessage(){ Author = "user2", Content = "Message8"},
            new ChatMessage(){ Author = "user3", Content = "Message9"},
        ];
    }
}