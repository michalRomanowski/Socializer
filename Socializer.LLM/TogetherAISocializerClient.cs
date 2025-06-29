using Socializer.Database.Models;
using System.Globalization;
using System.Text;
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

        public async Task<IEnumerable<Preference>> GetPreferences(string prompt)
        {
            try
            {

                var preferencesPrompt = new StringBuilder("From this text:");
                preferencesPrompt.AppendLine($"\"{prompt}\"");
                preferencesPrompt.AppendLine("Extract properties of types: \"interest\", \"activity\", \"hobby\", \"knownLanguage\", for each property link to http://dbpedia.org in form of .csv.");
                preferencesPrompt.AppendLine("Return only list in form:");
                preferencesPrompt.AppendLine("type, link");
                preferencesPrompt.AppendLine("and nothing else.");

                var response = await QueryAsync(preferencesPrompt.ToString());

                var preferences = new List<Preference>();

                foreach (var p in response.Split('\n'))
                {
                    var split = p.Split(',');

                    var type = (EPreferenceType)Enum.Parse(typeof(EPreferenceType), split[0], true);
                    var link = split[1];

                    preferences.Add(
                        new Preference() 
                        {
                            PreferenceType = type,
                            Url = link,
                        });
                }

                return preferences;
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public void Dispose() => client?.Dispose();
    }
}
