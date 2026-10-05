using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "key here";

    static async Task Main(string[] args)
    {
        Console.Write("Bir metin giriniz : ");
        string input = Console.ReadLine();
        Console.WriteLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Gelismis Duygu Analizi Yapiliyor...");
            string sentiment = await AdvancedSentimentAnalysis(input);
            Console.WriteLine();
            Console.WriteLine($"\n Sonuc : \n{sentiment}");
        }
    }

    static async Task<string> AdvancedSentimentAnalysis(string text)
    {
        using(HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization",$"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-5.6-luna",
                messages = new[]
                {
                    new {role="system",content="You are a advanced AI that analyses emotions in text. Your response must be JSON format. Identify the sentiment scores (0-100%) for the following emotions : Joy , Sadness , Anger , Fear , Surprise and Neutral"},
                    new {role="user" , content = $"Analyze this text : \"{text}\" and return a JSON object with percentages for each emotions."}
                }
            };
            
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json,Encoding.UTF8,"application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions",content);
            string responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                string analysis = result.choices[0].message.content.ToString();
                return analysis;
            }
            else
            {
                Console.WriteLine($"Bir hata olustu : {responseJson}");
                return "HATA"; 
            }
        }

        
    }
}