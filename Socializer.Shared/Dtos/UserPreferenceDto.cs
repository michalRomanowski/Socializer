namespace Socializer.Shared.Dtos;

public class UserPreferenceDto
{
    public Guid Id { get; set; }
    public string DBPediaResource { get; set; }
    public int Count { get; set; }
    public float Weight { get; set; }
}
