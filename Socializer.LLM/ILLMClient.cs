using System.Text;

namespace Socializer.LLM;

public interface ILLMClient : IDisposable
{
    Task<string> QueryAsync(StringBuilder prompt, string? context = default);
}
