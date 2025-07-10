using Microsoft.EntityFrameworkCore;

namespace Socializer.Database.Models;

[Index(nameof(Username), IsUnique = true)]
public class User : Entity
{
    public string Email { get; set; }
    public string Username { get; set; }
    public List<UserPreference> UserPreferences { get; set; } = [];
}