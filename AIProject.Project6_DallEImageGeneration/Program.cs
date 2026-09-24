
using Newtonsoft.Json;
using System.Text;

class Program
{
    public static async Task Main(string[] args)
    {
        string apiKey = "key";

        Console.WriteLine("Prompt giriniz:");
        string prompt = Console.ReadLine();

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add(
                "Authorization",
                $"Bearer {apiKey}"
            );

            var requestBody = new
            {
                model = "gpt-image-2",
                prompt = prompt,
                size = "1024x1024"
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(
                jsonBody,
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await client.PostAsync(
                "https://api.openai.com/v1/images/generations",
                content
            );

            string responseString =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("API HATASI:");
                Console.WriteLine(responseString);
                return;
            }

            dynamic result = JsonConvert.DeserializeObject(responseString);

            string base64Image = result.data[0].b64_json;
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            string fileName = "generated_image.png";

            await File.WriteAllBytesAsync(fileName, imageBytes);

            Console.WriteLine("Görsel başarıyla oluşturuldu!");
            Console.WriteLine($"Dosya: {Path.GetFullPath(fileName)}");
        }
    }
}