using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.CastDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminCastController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public AdminCastController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
        }

        public async Task<IActionResult> CastList()
        {
            ViewBag.v1 = "Oyuncu Yönetimi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Oyuncular & Yönetmenler";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/Casts");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var casts = JsonConvert.DeserializeObject<List<ResultCastDto>>(json) ?? new();
                return View(casts);
            }

            return View(new List<ResultCastDto>());
        }
    }
}
