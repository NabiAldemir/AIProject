using Newtonsoft.Json;
using System.Text;

namespace AIProject.Project20_RecipeSuggestionWithOpenAI.Models
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private const string openAIUrl = "https://api.openai.com/v1/chat/completions";
        private const string apiKey = "key here";

        public OpenAiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {apiKey}");
        }

        public async Task<string> GetRecipeAsync(string ingredients)
        {
            var requestBody = new
            {
                model = "gpt-5.6-luna",
                messages = new[]
                {
                    new{role="system",content = "Sen profesyonel bir ascisin. Kullanicinin elindeki malzemelere gore yemek tarifi oner."},
                    new{role="user",content = $"Elimde {ingredients} malzemeleri var. Bana yemek oner."}
                }
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var jsonContent = new StringContent(jsonRequest,Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync(openAIUrl, jsonContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return result.choices[0].message.content;
            }
            else
            {
                Console.WriteLine($"HTTP Hata Kodu: {(int)response.StatusCode}");
                Console.WriteLine($"Hata Detayı: {responseBody}");
                return responseBody;
            }
        }
    }
}
