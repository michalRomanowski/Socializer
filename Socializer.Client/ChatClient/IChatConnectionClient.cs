using Common.Utils;

namespace Socializer.Client.ChatClient;

public interface IChatConnectionClient : IAsyncDisposable
{
    Task<OperationResult<bool>> InitAsync(Func<string, string, Task> onReceiveMessage);
    Task<OperationResult<bool>> SendMessageAsync(string author, string content);
}
