namespace Socializer.BlazorHybrid.ViewModels;

internal class ChatViewModel
{
    public string NewMessage { get; set; } = string.Empty;

    public List<ChatMessage> Messages { get; set; } = [];
}