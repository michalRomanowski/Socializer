namespace Socializer.BlazorHybrid.ViewModels;

internal class ChatMessage
{
    public string Author { get; set; }
    public string Content { get; set; }
}

internal class ChatViewModel
{
    public string NewMessage { get; set; } = string.Empty;

    public List<ChatMessage> Messages { get; set; } = [];
}