namespace Socializer.Database.Models;

public class User : Entity
{
    public string Email { get; set; }
    public string Username { get; set; }
    public List<UserPreference> UserPreferences { get; set; } = [];
}