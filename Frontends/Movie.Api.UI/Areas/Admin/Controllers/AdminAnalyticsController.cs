using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminAnalyticsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public AdminAnalyticsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
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
