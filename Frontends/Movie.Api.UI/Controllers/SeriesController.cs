using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Controllers
{
    public class SeriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public SeriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> SeriesList()
        {
            ViewBag.v1 = "Dizi Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Diziler";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/Series");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(json);
                return View(values);
            }
            return View(new List<AdminResultSeriesDTO>());
        }

        public async Task<IActionResult> SeriesDetail(int id)
        {
            ViewBag.id = id;
            ViewBag.SeriesID = id;
            return View();
        }
    }
}
