using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MvcKutuphane.Controllers
{
    public class AIController : Controller
    {
        public async Task<ActionResult> Index(string userPrompt)
        {
            if (string.IsNullOrWhiteSpace(userPrompt))
            {
                return View();
            }

            string apiKey = ConfigurationManager.AppSettings["OpenAIKey"];
            string apiUrl = ConfigurationManager.AppSettings["OpenAIUrl"];

            var systemPrompt =
                "Sen bir kitap öneri asistanısın. " +
                "Görevin, kullanıcının isteklerini, ilgi alanlarını, okuma alışkanlıklarını " +
                "ve belirttiği kriterleri anlayarak ona uygun kitaplar önermektir. " +
                "Kullanıcının söylediği kriterleri dikkatlice analiz et. " +
                "Tür, konu, tema, yazar, dönem, uzunluk, dil ve yaş grubu gibi kriterleri dikkate al. " +
                "Kullanıcı belirli bir kitap, film, dizi veya yazar üzerinden öneri isterse benzer özelliklere sahip kitaplar öner. " +
                "Kullanıcı yeterli bilgi vermediyse kısa ve doğal bir soru sor. " +
                "Her öneri için kitabın adını, yazarını ve neden uygun olduğunu belirt. " +
                "Kitap hakkında emin olmadığın bilgileri uydurma. " +
                "Yanıtların doğal, samimi ve yardımcı bir tonda olsun.";

            var requestData = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = systemPrompt
                    },
                    new
                    {
                        role = "user",
                        content = userPrompt
                    }
                },
                temperature = 0.7
            };

            string json = JsonConvert.SerializeObject(requestData);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    ViewBag.Error = "API Hatası: " + error;
                    return View();
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                var result =
                    JsonConvert.DeserializeObject<OpenAiResponse>(responseJson);

                string answer = result?.Choices?[0]?.Message?.Content;

                ViewBag.Answer = answer;

                return View();
            }
        }
    }

    public class OpenAiResponse
    {
        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        [JsonProperty("message")]
        public OpenAiMessage Message { get; set; }
    }

    public class OpenAiMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}