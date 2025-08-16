using Microsoft.AspNetCore.Components;

namespace Socializer.BlazorShared.Extensions;

internal static class MarkupStringExtensions
{
    public static MarkupString ToHtmlBreaks(this string str)
    {
        return new MarkupString(str.Replace("\n", "<br>").Replace("\r", string.Empty));
    }
}
