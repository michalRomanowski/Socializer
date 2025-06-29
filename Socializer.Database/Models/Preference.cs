namespace Socializer.Database.Models;

public class Preference : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public EPreferenceType PreferenceType { get; set; }
    public string Url { get; set; }
}