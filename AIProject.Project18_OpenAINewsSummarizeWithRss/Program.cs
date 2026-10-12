using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

class Program
{
    private static readonly string apiKey = "key here";
    private static readonly string rssFeedUrl = "link here";

    static async Task Main()
    {
        Console.WriteLine("Haberler Sistemden Aliniyor...");
        List<string> articles = await FetchLatestNews(10);
        foreach (var article in articles) 
        {
            Console.WriteLine("Haber ozeti olusturuluyor...");
            string summary = await SummarizeArticle(article);
            Console.WriteLine("AI Tarafindan Ozetlenen Haber : ");
            Console.WriteLine(summary);
            Console.WriteLine("----------------------------------------------------------------------------------\n");
        }
    }

    static async Task<List<string>> FetchLatestNews(int count)
    {
        var client = new HttpClient();
        string rssContent = await client.GetStringAsync(rssFeedUrl);

        XDocument doc = XDocument.Parse(rssContent);
        var items = doc.Descendants("item").Take(count);

        List<string> articles = items.Select(item =>
        {
            string title = item.Element("title")?.Value ?? "";
            string description = item.Element("description")?.Value ?? "";
            return $"{title}. {description}";
        }).ToList();
        return articles;
    }

    static async Task<string> SummarizeArticle(string articleText)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var requestBody = new
        {
            model = "gpt-5.6-luna",
            messages = new[]
            {
                new {role = "system", content = "You are an expert news summarizer."},
                new {role = "user", content = "Bu haberi 3 cumlede ozetle:" + articleText}
            },
            max_completion_tokens = 500
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody),Encoding.UTF8,"application/json");
        HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return result.choices[0].message.content;
        }
        else
        {
            Console.WriteLine("Hata : " + responseContent);
            return "Hata";
        }
    }
}