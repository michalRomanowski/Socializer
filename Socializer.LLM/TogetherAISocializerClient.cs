using Microsoft.Extensions.Logging;
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

        public async Task<string> QueryAsync(string prompt, int maxTokens)
        {
            if(maxTokens != default)
                prompt += $". Keep response within {maxTokens} tokens limit.";

            var messages = new List<TogetherAIChatMessage>
            {
                new TogetherAIChatUserMessage(prompt)
            };

            // Setup Request Arguments
            var chatArgs = new TogetherAIChatCompletionArgs
            {
                Model = settings.Model,
                Stop = [
                "</s>",
                "[/INST]"
            ],
                MaxTokens = maxTokens,
                Messages = messages,
            };

            // Getting result
            var result = await client.GetChatCompletionsAsync(chatArgs);

            logger.LogInformation("Received response from Together AI with CompletionTokens: {CompletionTokens} TotalTokens: {TotalTokens} MaxTokens: {}.", result.Usage.CompletionTokens, result.Usage.TotalTokens, maxTokens);

            return result?.Choices?.FirstOrDefault()?.Message?.Content ?? throw new NotImplementedException(); // TODO: Some better exception
        }

        public void Dispose() => client?.Dispose();
    }
}
