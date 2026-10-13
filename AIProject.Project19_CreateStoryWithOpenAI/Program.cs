using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

class Program
{
    private static readonly string apiKey = "key here";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Hikaye Turunu Seciniz (Macera,Korku,Bilim Kurgu,Fantastik,Komedi): ");
        string genre = Console.ReadLine();

        Console.WriteLine("Ana karakteriniz kim: ");
        string character = Console.ReadLine();

        Console.WriteLine("Hikaye nerede geciyor: ");
        string place = Console.ReadLine();

        Console.WriteLine("Hikayenin uzunlugu (kisa/orta/uzun): ");
        string length = Console.ReadLine();

        string prompt = $"{genre} turunde bir hikaye yaz. Bas karakterin adi {character}. Hikaye {place} bolgesinde geciyor. {length} uzunlugunda bir hikaye olsun. Giris,gelisme ve sonuc icermeli. ";

        string story = await GenerateStory(prompt);
        Console.WriteLine();
        Console.WriteLine("---AI Tarafindan Olusturulan Hikaye---\n\n");
        Console.WriteLine(story);
    }

    static async Task<string> GenerateStory(string prompt)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var requestBody = new
        {
            model = "gpt-5.6-luna",
            messages = new[]
            {
                new {role="system",content = "You are a creative story writer"},
                new {role="user",content = prompt}
            }
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody),Encoding.UTF8,"application/json");
        HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions",jsonContent);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return result.choices[0].message.content;
        }
        else
        {
            Console.WriteLine($"Hata : ", responseContent);
            return "Hata";
        }
    }
}