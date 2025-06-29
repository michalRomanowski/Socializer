using Together.AI;

namespace Socializer.LLM
{
    public class TogetherAISocializerClient : ILLMClient
    {
        private readonly TogetherAISettings settings;
        private readonly TogetherAIClient client;

        public TogetherAISocializerClient(TogetherAISettings settings)
        {
            this.settings = settings;

            var httpClient = new HttpClient();
            httpClient.SetupClient(settings.ApiKey);

            client = new TogetherAIClient(httpClient);
        }

        public async Task<string> QueryAsync(string prompt)
        {
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
                MaxTokens = 512,
                Messages = messages,
            };

            // Getting result
            var result = await client.GetChatCompletionsAsync(chatArgs);

            return result?.Choices?.FirstOrDefault()?.Message?.Content ?? throw new NotImplementedException(); // TODO: Some better exception
        }

        public void Dispose() => client?.Dispose();
    }
}
