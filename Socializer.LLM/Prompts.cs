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

        public static StringBuilder PreferencesPrompt(string prompt)
        {
            var sb = new StringBuilder("From this text:");
            sb.AppendLine($"\"{prompt}\"");
            sb.AppendLine($"Extract properties corresponding to text of types: {preferenceTypesText}, for each property add link to http://dbpedia.org.");
            sb.AppendLine("Return only list in form:");
            sb.AppendLine("type, link");
            sb.AppendLine("and nothing else.");

            return sb;
        }
    }
}
