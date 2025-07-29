namespace Socializer.Shared.Extensions;

public static class ChatHashExtensions
{
    public static string GenerateChatHash(this Guid userId)
    {
        return userId.ToString();
    }

    public static string GenerateChatHash(this IEnumerable<Guid> userIds)
    {
        var orderedUserIds = userIds.Order().ToList();

        // This is good enough. We will support 2 users + bot groups total
        var hash = string.Join('_', orderedUserIds);
        return hash;
    }
}
