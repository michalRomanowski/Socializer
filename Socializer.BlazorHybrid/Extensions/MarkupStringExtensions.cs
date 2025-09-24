using Microsoft.AspNetCore.Components;

namespace Socializer.BlazorHybrid.Extensions;

internal static class MarkupStringExtensions
{
    public static MarkupString ToHtmlBreaks(this string str) =>
        new(string.IsNullOrEmpty(str) ? string.Empty : str.Replace("\n", "<br>").Replace("\r", string.Empty));
}
