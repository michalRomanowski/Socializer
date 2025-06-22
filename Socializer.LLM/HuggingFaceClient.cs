using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Socializer.LLM;

public class HuggingFaceClient : ILLMClient
{
    private readonly HuggingFaceSettings settings;
    private readonly HttpClient httpClient;
    
    public HuggingFaceClient(HuggingFaceSettings settings)
    {
        this.settings = settings;

        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
    }

    public async Task<string> QueryAsync(string prompt)
    {
        var url = $"https://api-inference.huggingface.co/models/{settings.Model}";

        var content = new
        {
            inputs = prompt
        };

        var response = await httpClient.PostAsync(url, new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        var responseText = await response.Content.ReadAsStringAsync();
        return responseText;
    }

    public void Dispose() => httpClient?.Dispose();
}