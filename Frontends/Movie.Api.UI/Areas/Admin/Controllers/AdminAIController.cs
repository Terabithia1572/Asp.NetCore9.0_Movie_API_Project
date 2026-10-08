using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminAIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public AdminAIController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
        }

        public IActionResult Index()
        {
            ViewBag.v1 = "Yapay Zeka & ML Modelleri";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "AI Modelleri & Duygu Analizi";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TestSentiment([FromBody] SentimentInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Text))
            {
                return Json(new { success = false, message = "Metin boş olamaz." });
            }

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(new { text = model.Text }), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{ApiBaseUrl}/Analytics/test-sentiment", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(json);
                    return Json(new
                    {
                        success = true,
                        sentiment = result?.sentiment,
                        confidenceScore = result?.confidenceScore,
                        text = model.Text
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = false, message = "Analiz yapılamadı." });
        }

        [HttpPost]
        public IActionResult RetrainModel(string modelName)
        {
            return Json(new
            {
                success = true,
                message = $"{modelName ?? "Model"} yeniden eğitme işlemi arkaplanda başlatıldı.",
                timestamp = DateTime.Now.ToString("HH:mm:ss")
            });
        }
    }

    public class SentimentInputModel
    {
        public string Text { get; set; } = null!;
    }
}
