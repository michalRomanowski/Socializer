namespace Socializer.Shared.Dtos;

public class ChatDto : Dto
{
    public string ChatHash { get; set; }
    public IEnumerable<string> Usernames { get; set; }
}