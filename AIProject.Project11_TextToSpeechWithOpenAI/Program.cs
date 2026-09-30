using Newtonsoft.Json;
using System.Text;

class Program
{
    static readonly string apiKey = "key";

    static async Task Main(string[] args)
    {
        Console.Write("Metni Giriniz: ");
        string input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyasi olusturuluyor...");
            await GenerateSpeech(input);
            Console.Write("Ses dosyasi 'output.mp3' olarak olusturuldu! ");
            System.Diagnostics.Process.Start("explorer.exe","output.mp3");
        }

    }

    static async Task GenerateSpeech(string text) 
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-4o-mini-tts",
                input = text,
                voice = "alloy",
                instructions = "Speak naturally, clearly, and with a friendly tone."
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json,Encoding.UTF8,"application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech",content);

            if (response.IsSuccessStatusCode) 
            {
                byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync("output.mp3", audioBytes);
            }
            else
            {
                Console.WriteLine("Bir hata olustu...");
            }
        }
    }
}