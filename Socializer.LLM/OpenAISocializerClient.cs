using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;
using System.Text;

namespace Socializer.LLM;

internal class OpenAISocializerClient(OpenAISettings settings, ILogger<OpenAISocializerClient> logger) : ILLMClient
{
    public async Task<string> QueryAsync(StringBuilder prompt, string? context = null)
    {
        logger.LogInformation("Sending prompt to OpenAI: {prompt}.", prompt);

        var client = new OpenAIClient(settings.ApiKey).GetChatClient(settings.Model).AsIChatClient();

        var response = await client.GetResponseAsync(prompt.ToString());

        var responseText = response.Text;

        logger.LogInformation(
            "Received response from OpenAI: '{responseText}' outputTokens: {outputTokenCount}.",
            responseText,
            response.Usage.OutputTokenCount);

        return responseText;
    }

    public void Dispose(){ }
}
