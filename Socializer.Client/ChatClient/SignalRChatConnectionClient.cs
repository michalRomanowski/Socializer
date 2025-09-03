using Common.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Storage;
using Socializer.Shared;

namespace Socializer.Client.ChatClient;

internal class SignalRChatConnectionClient(SharedSettings settings) : IChatConnectionClient
{
    private HubConnection? hubConnection;
    private readonly string chatHubUrl = settings.SocializerChatApiUrl + "/chathub"; // TODO: Move to some const as also used in auth configuration, now hardcoded in both places

    public async Task<OperationResult<bool>> InitAsync(Func<string, string, Task> onReceiveMessage)
    {
        try
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(chatHubUrl, options =>
                {
                    {
                        options.AccessTokenProvider = async () =>
                        {
                            return await SecureStorage.Default.GetAsync("access_token"); // TODO: Handle token refreshment/expiry
                        };
                    }
                })
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On(
                "ReceiveMessage", onReceiveMessage);

            await hubConnection.StartAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }

    public async Task<OperationResult<bool>> JoinGroupAsync(string chatHash)
    {
        try
        {
            await hubConnection.SendAsync("JoinGroup", chatHash);
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }

    public async Task<OperationResult<bool>> SendMessageAsync(Guid authorId, string chatHash, string content)
    {
        try
        {
            await hubConnection.SendAsync("SendMessage", authorId, chatHash, content);
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure(ex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is null)
        {
            return;
        }

        await hubConnection.DisposeAsync();
    }
}
