using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAnalyticsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminAnalyticsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Veri Analitiği";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "İstatistikler & Grafikler";

            var client = _httpClientFactory.CreateClient();
            string jsonResult = "{}";

            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Analytics/summary");
                if (response.IsSuccessStatusCode)
                {
                    jsonResult = await response.Content.ReadAsStringAsync();
                }
            }
            catch { }

            ViewBag.AnalyticsJson = jsonResult;
            return View();
        }
    }
}
