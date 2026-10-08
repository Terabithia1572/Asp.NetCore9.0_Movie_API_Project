using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.DashboardDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public AdminDashboardController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Yönetim Paneli";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "İstatistikler & Özet";

            var client = _httpClientFactory.CreateClient();
            var stats = new ResultDashboardStatsDto();

            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Dashboard/stats");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    stats = JsonConvert.DeserializeObject<ResultDashboardStatsDto>(json) ?? new();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "İstatistikler yüklenirken hata oluştu: " + ex.Message;
            }

            return View(stats);
        }
    }
}
