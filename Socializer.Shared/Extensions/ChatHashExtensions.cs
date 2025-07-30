namespace Socializer.Shared.Extensions;

// TODO: Can be service
public static class ChatHashExtensions
{
    private const char separator = '_';

    public static string GenerateChatHash(this Guid userId)
    {
        return userId.ToString();
    }

    public static string GenerateChatHash(this ISet<Guid> userIds)
    {
        var orderedUserIds = userIds.Order().ToList();

        // This is good enough. We will support 2 users + 1 bot groups total
        var hash = string.Join('_', orderedUserIds);
        return hash;
    }

    public static ISet<Guid> SplitChatHash(this string chatHash)
    {
        return chatHash.Split(separator).Select(x => new Guid(x)).ToHashSet();
    }
}
