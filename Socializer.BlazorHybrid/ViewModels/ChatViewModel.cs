namespace Socializer.BlazorHybrid.ViewModels
{
    internal class ChatMessage
    {
        public string Author { get; set; }
        public string Content { get; set; }
    }

    internal class ChatViewModel
    {
        public string? NewMessage { get; set; }

        public List<ChatMessage> Messages { get; set; } = [ 
            new ChatMessage(){ Author = "user1", Content = "Message1"},
            new ChatMessage(){ Author = "user2", Content = "Message2"},
            new ChatMessage(){ Author = "user2", Content = "Message3"},
            new ChatMessage(){ Author = "user3", Content = "Message4"},
            new ChatMessage(){ Author = "user1", Content = "Message5"},
            new ChatMessage(){ Author = "user1", Content = "Message6"},
            new ChatMessage(){ Author = "user1", Content = "Message7"},
            new ChatMessage(){ Author = "user2", Content = "Message8"},
            new ChatMessage(){ Author = "user3", Content = "Message9"},
        ];
    }
}
