using Newtonsoft.Json;
using System.Security.Principal;
using System.Text;

class Program
{
    private static readonly string apiKey = "key here";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Uzun Metninizi veya Makalenizi Giriniz : ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine();
            Console.WriteLine("Girmis oldugunuz metin AI tarafindan ozetleniyor...");
            Console.WriteLine();

            string shortSummary = await SummarizeText(input,"short");
            string mediumSummary = await SummarizeText(input, "medium");
            string detailedSummary = await SummarizeText(input, "detailed");

            Console.WriteLine("Ozetler : ");
            Console.WriteLine("-------------------------------");
            Console.WriteLine($" ** Kisa Ozet : ** {shortSummary}");
            Console.WriteLine("-------------------------------");
            Console.WriteLine($" ** Orta Uzunlukta Ozet : ** {mediumSummary}");
            Console.WriteLine("-------------------------------");
            Console.WriteLine($" ** Detayli Ozet : ** {detailedSummary}");
        }
    }

    static async Task<string> SummarizeText(string text,string level) 
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization",$"Bearer {apiKey}");

            string instructions = level switch
            {
                "short" => "Summarize this text in 1-2 sentences",
                "medium" => "Summarize this text in 3-5 sentences",
                "detailed" => "Summarize this text in a detailed but concise manner",
                _ => "Summarize this text."
            };

            var requestBody = new
            {
                model = "gpt-5.6-luna",
                messages = new[]
                {
                    new {role="system",content="You are an AI that summarize text info different levels : short,medium and detailed"},
                    new {role="user",content=$"{instructions} \n\n {text}"},
                }
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json,Encoding.UTF8,"application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions",content);
            string responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                return result.choices[0].message.content.ToString();
            }
            else
            {
                Console.WriteLine($"Bir Hata Olustu! {responseJson}");
                return "Hata";
            }
        }
    }
}

// Sample : Kış mevsimi, soğuk hava ve kısa günlerin yaşandığı bir mevsimdir. Bu dönemde hava sıcaklıkları düşer ve doğada birçok değişiklik meydana gelir. Kış mevsiminde bazı bitkiler soğuk hava nedeniyle büyümekte zorlanır. Ancak bazı sebzeler düşük sıcaklıklara dayanıklı oldukları için kışın yetiştirilebilir. Lahana, karnabahar ve brokoli kış aylarında yetiştirilen önemli sebzelerdendir. Ispanak da soğuğa dayanıklı olması nedeniyle kışın sıkça tüketilir. Pırasa, kış mevsiminin en çok tercih edilen sebzelerinden biridir. Havuç ve turp da soğuk havalarda yetişebilen kök sebzelerdendir. Brüksel lahanası da özellikle serin ve soğuk havalarda iyi gelişir. Kış sebzeleri, insan sağlığı açısından birçok önemli vitamin ve mineral içerir. Örneğin ıspanak demir, folat ve çeşitli vitaminler bakımından zengindir. Brokoli ve karnabahar ise C vitamini ve lif açısından önemli besinlerdir. Lahana, bağışıklık sisteminin desteklenmesine yardımcı olan besin maddeleri içerir. Kış sebzelerinin düzenli tüketilmesi dengeli beslenmeye katkı sağlar. Bu sebzeler çorba, yemek, salata ve çeşitli garnitürlerde kullanılabilir. Kışın sebze yetiştiriciliğinde don olaylarına karşı dikkatli olunması gerekir. Özellikle seralarda uygun sıcaklık ve nem koşullarının sağlanması önemlidir. Çiftçiler, yetiştirilecek sebzeleri bölgenin iklim özelliklerine göre seçmelidir. Kış sebzeleri hem sağlıklı beslenme hem de tarımsal üretim açısından büyük önem taşır. Bu nedenle kış mevsimi, birçok dayanıklı sebzenin yetiştirildiği verimli bir dönemdir.
