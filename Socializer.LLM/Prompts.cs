using System.Text;

namespace Socializer.LLM;

public static class Prompts
{
    public static StringBuilder AppendMessage(this StringBuilder sb, string message)
    {
        sb.AppendLine($"Message='{message}'");
        return sb;
    }

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

    public static StringBuilder AppendSafetyCensorship(this StringBuilder sb)
    {
        sb.AppendLine($"Don't include non Family Friendly resources.");
        return sb;
    }

    public static StringBuilder AppendResponseCharsLimit(this StringBuilder sb, int charsLimit)
    {
        sb.AppendLine($"Keep response shorter than {charsLimit} characters.");
        return sb;
    }

    public static StringBuilder PreferencesPrompt(string prompt)
    {
        var sb = new StringBuilder("In this text:");
        sb.AppendLine($"\"{prompt}\"");
        sb.AppendLine($"Find mentoined activities and interests.");
        sb.AppendLine("Return only list in form:");
        sb.AppendLine($"RESOURCE1");
        sb.AppendLine($"RESOURCE2");
        sb.AppendLine($"Where RESOURCE is dbpedia resource uri without prefix 'http://dbpedia.org/resource/' corresponding to found activity or interest");
        sb.AppendLine("and nothing else.");
        sb.AppendSafetyCensorship();
        sb.AppendResponseCharsLimit(300); // TODO: Make configurable

        return sb;
    }
}
