using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.SeasonDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSeasonController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSeasonController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task LoadSeriesTitleAsync(int seriesId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:44319/api/Series/GetMovile?id={seriesId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var series = JsonConvert.DeserializeObject<AdminResultSeriesDTO>(json);
                    ViewBag.SeriesTitle = series?.SeriesTitle ?? $"Dizi #{seriesId}";
                }
                else
                {
                    ViewBag.SeriesTitle = $"Dizi #{seriesId}";
                }
            }
            catch
            {
                ViewBag.SeriesTitle = $"Dizi #{seriesId}";
            }
        }

        public async Task<IActionResult> SeasonList(int seriesId)
        {
            ViewBag.v1 = "Sezon Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Dizi Sezonları";
            ViewBag.SeriesID = seriesId;
            await LoadSeriesTitleAsync(seriesId);

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44319/api/seasons/series/{seriesId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultSeasonDto>>(jsonData) ?? new();
                return View(values);
            }
            return View(new List<ResultSeasonDto>());
        }

        [HttpGet]
        public async Task<IActionResult> CreateSeason(int seriesId)
        {
            ViewBag.v1 = "Sezon Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Sezon Ekleme";
            ViewBag.SeriesID = seriesId;
            await LoadSeriesTitleAsync(seriesId);

            var dto = new CreateSeasonDto
            {
                SeriesID = seriesId
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeason(CreateSeasonDto dto)
        {
            ViewBag.SeriesID = dto.SeriesID;
            await LoadSeriesTitleAsync(dto.SeriesID);

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:44319/api/seasons", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("SeasonList", new { seriesId = dto.SeriesID });
            }

            var errJson = await responseMessage.Content.ReadAsStringAsync();
            ModelState.AddModelError("", errJson);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSeason(int id)
        {
            ViewBag.v1 = "Sezon Güncelleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Sezon Güncelleme";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44319/api/seasons/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var season = JsonConvert.DeserializeObject<ResultSeasonDto>(jsonData);
                if (season != null)
                {
                    ViewBag.SeriesID = season.SeriesID;
                    await LoadSeriesTitleAsync(season.SeriesID);

                    var updateDto = new UpdateSeasonDto
                    {
                        SeasonID = season.SeasonID,
                        SeriesID = season.SeriesID,
                        SeasonNumber = season.SeasonNumber,
                        Overview = season.Overview,
                        AirDate = season.AirDate,
                        PosterImageUrl = season.PosterImageUrl
                    };
                    return View(updateDto);
                }
            }
            return RedirectToAction("SeriesList", "AdminSeries");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeason(UpdateSeasonDto dto)
        {
            ViewBag.SeriesID = dto.SeriesID;
            await LoadSeriesTitleAsync(dto.SeriesID);

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:44319/api/seasons", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("SeasonList", new { seriesId = dto.SeriesID });
            }

            var errJson = await responseMessage.Content.ReadAsStringAsync();
            ModelState.AddModelError("", errJson);
            return View(dto);
        }

        public async Task<IActionResult> DeleteSeason(int id, int seriesId)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:44319/api/seasons/{id}");
            return RedirectToAction("SeasonList", new { seriesId = seriesId });
        }
    }
}
