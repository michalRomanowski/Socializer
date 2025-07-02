using Microsoft.Extensions.Logging;
using System.Text;
using Together.AI;

namespace Socializer.LLM
{
    public class TogetherAISocializerClient : ILLMClient
    {
        private readonly TogetherAISettings settings;
        private readonly ILogger<TogetherAISocializerClient> logger;
        private readonly TogetherAIClient client;

        public TogetherAISocializerClient(TogetherAISettings settings, ILogger<TogetherAISocializerClient> logger)
        {
            this.settings = settings;
            this.logger = logger;

            var httpClient = new HttpClient();
            httpClient.SetupClient(settings.ApiKey);

            client = new TogetherAIClient(httpClient);
        }

        public async Task<string> QueryAsync(StringBuilder prompt, int tokenLimit, string? context)
        {
            if(tokenLimit != default)
                prompt.AppendTokenLimit(tokenLimit);

            var messages = new List<TogetherAIChatMessage>
            {
                new TogetherAIChatUserMessage(prompt.ToString())
            };

            // Setup Request Arguments
            var chatArgs = new TogetherAIChatCompletionArgs
            {
                Model = settings.Model,
                Stop = [
                "</s>",
                "[/INST]"
            ],
                MaxTokens = tokenLimit,
                Messages = messages
            };

            // Getting result
            var result = await client.GetChatCompletionsAsync(chatArgs);

            logger.LogInformation("Received response from Together AI with CompletionTokens: {CompletionTokens} TotalTokens: {TotalTokens} MaxTokens: {}.", result.Usage.CompletionTokens, result.Usage.TotalTokens, tokenLimit);

            return result?.Choices?.FirstOrDefault()?.Message?.Content ?? throw new NotImplementedException(); // TODO: Some better exception
        }

        public void Dispose() => client?.Dispose();
    }
}
