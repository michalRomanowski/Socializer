using System.Text;

namespace Socializer.LLM
{
    public static class Messages
    {
        public static StringBuilder HelloMessage()
        {
            return new StringBuilder($"Hello! Tell me something about yourself. What do you enjoy? What interests you? What would you like to do with someone?");
        }
    }
}
