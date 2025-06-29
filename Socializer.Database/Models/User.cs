namespace Socializer.Database.Models;

public class User : Entity
{
    public List<Preference> Preferences { get; set; } = [];

    public string Email { get; set; }
    public string Username { get; set; }
}