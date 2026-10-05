using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.CastDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCastController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminCastController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
