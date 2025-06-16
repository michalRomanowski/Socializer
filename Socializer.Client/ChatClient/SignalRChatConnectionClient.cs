using Common.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Socializer.Shared;

namespace Socializer.Client.ChatClient
{
    internal class SignalRChatConnectionClient(SharedSettings settings) : IChatConnectionClient
    {
        private HubConnection? hubConnection;
        private readonly string chatHubUrl = settings.SocializerApiUrl + "/chathub";

        public async Task<OperationResult<bool>> InitAsync(Func<string, string, Task> onReceiveMessage)
        {
            try
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(chatHubUrl)
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

        public async Task<OperationResult<bool>> SendMessageAsync(string author, string content)
        {
            try
            {
                await hubConnection.SendAsync("SendMessage", author, content);
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
}
