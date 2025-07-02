using System.Text;

namespace Socializer.LLM
{
    public static class Messages
    {
        public static StringBuilder HelloMessage(string username)
        {
            return new StringBuilder($"Hello {username}! Tell me something about yourself. What do you enjoy? What interests you? What would you like to do with someone?");
        }
    }
}
