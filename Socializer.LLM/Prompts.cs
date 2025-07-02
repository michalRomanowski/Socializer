using Socializer.Database.Models;
using System.Text;

namespace Socializer.LLM
{
    public static class Prompts
    {
        private static readonly string preferenceTypesText = string.Join(", ", Enum.GetNames(typeof(EPreferenceType)).Select(x => $"\"{x}\""));

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

        public static StringBuilder PreferencesPrompt(string prompt)
        {
            var sb = new StringBuilder("From this text:");
            sb.AppendLine($"\"{prompt}\"");
            sb.AppendSafetyCensorship();
            sb.AppendLine($"Find http://dbpedia.org resources for up to 10 {preferenceTypesText} properties corresponding to text.");
            sb.AppendLine($"Prioritize {EPreferenceType.Activity}.");
            sb.AppendLine("Return only list in form:");
            sb.AppendLine($"type, resource");
            sb.AppendLine($"Where type is value from list: {preferenceTypesText}. Resource is dbpedia resource uri without prefix 'http://dbpedia.org/resource/'");
            sb.AppendLine("and nothing else.");
            sb.AppendTokenLimit(100); // TODO: Configurable

            return sb;
        }
    }
}
