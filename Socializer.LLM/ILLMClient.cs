using Socializer.Database.Models;

namespace Socializer.LLM
{
    public interface ILLMClient : IDisposable
    {
        Task<string> QueryAsync(string prompt);
        Task<IEnumerable<Preference>> GetPreferences(string prompt); // TODO: In future we might want to use different models/approaches for preferences and split this client
    }
}
