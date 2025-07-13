using System.Text;

namespace Socializer.LLM;

public static class Prompts
{
    public static StringBuilder AppendHelpFindMoreInterests(this StringBuilder sb)
    {
        sb.AppendLine($"Help me to find more about interests mentioned in this message that I could share with other people. Encourage me to share more information.");
        return sb;
    }

    public static StringBuilder AppendSameLanguageResponse(this StringBuilder sb)
    {
        sb.AppendLine($"Respond in the same language.");
        return sb;
    }

    public static StringBuilder AppendTokenLimit(this StringBuilder sb, int tokenLimit)
    {
        sb.AppendLine($"Keep response within {tokenLimit} tokens limit.");
        return sb;
    }

    public static StringBuilder AppendSafetyCensorship(this StringBuilder sb)
    {
        sb.AppendLine($"Remove non Family Friendly parts.");
        return sb;
    }

    public static StringBuilder PreferencesPrompt(string prompt, int tokenLimit)
    {
        var sb = new StringBuilder("In this text:");
        sb.AppendLine($"\"{prompt}\"");
        sb.AppendSafetyCensorship();
        sb.AppendLine($"Find mentoined activities and interests.");
        sb.AppendLine("Return only list in form:");
        sb.AppendLine($"RESOURCE1");
        sb.AppendLine($"RESOURCE2");
        sb.AppendLine($"Where RESOURCE is dbpedia resource uri without prefix 'http://dbpedia.org/resource/' corresponding to foun activity or interest");
        sb.AppendLine("and nothing else.");
        sb.AppendTokenLimit(tokenLimit); // TODO: Configurable

        return sb;
    }
}
