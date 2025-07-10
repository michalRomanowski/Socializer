namespace Socializer.Database.Models;

public class UserPreference : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid PreferenceId { get; set; }
    public Preference Preference { get; set; }
    public int Count { get; set; }
    public float Weight { get; set; }
}
