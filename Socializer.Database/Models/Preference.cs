using Microsoft.EntityFrameworkCore;

namespace Socializer.Database.Models;

[Index(nameof(DBPediaResource), IsUnique = true)]
public class Preference : Entity
{
    public string DBPediaResource { get; set; }
    public List<UserPreference> UserPreferences { get; set; } = [];
}