namespace Socializer.BlazorShared.ViewModels;

public class ChatViewModel
{
    public string NewMessage { get; set; } = string.Empty;

    public List<ChatMessage> Messages { get; set; } = [];
}