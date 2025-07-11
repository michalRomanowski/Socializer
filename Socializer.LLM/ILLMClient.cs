using System.Text;

namespace Socializer.LLM;

public interface ILLMClient : IDisposable
{
    Task<string> QueryAsync(StringBuilder prompt, int tokenLimit = default, string? context = default);
}
