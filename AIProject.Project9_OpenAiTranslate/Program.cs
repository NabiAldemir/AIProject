using Newtonsoft.Json;
using System.Text;

class Program
{
    private static async Task Main(string[] args)
    {
        Console.Write("Lutfen cevirmek istediginiz metni giriniz : ");
        string inputText = Console.ReadLine();

        string apiKey = "key here";

        string translatedText = await TranslatedTextToEnglish(inputText, apiKey);

        if (!string.IsNullOrEmpty(translatedText))
        {
            Console.WriteLine();
            Console.Write($"Ceviri :{translatedText}");
            Console.WriteLine();
        }
        else
        {
            Console.Write("Beklenmeyen hata! ");
        }
    }

    private static async Task<string> TranslatedTextToEnglish(string text, string apiKey)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                model = "gpt-5.6-luna",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful translator." },
                    new { role = "user", content = $"Please translate this text to English : {text}" },
                }
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody,Encoding.UTF8,"application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseString = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseString);
                string translation = responseObject.choices[0].message.content;
                return translation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata olustu : {ex.Message}");
                return null;
            }
        }
    }
}