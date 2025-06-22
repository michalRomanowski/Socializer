namespace Socializer.LLM
{
    public interface ILLMClient : IDisposable
    {
        Task<string> QueryAsync(string prompt);
    }
}
