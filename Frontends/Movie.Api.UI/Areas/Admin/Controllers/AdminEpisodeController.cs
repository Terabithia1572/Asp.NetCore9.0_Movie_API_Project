using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.EpisodeDTOs;
using MovieApi.DTOs.DTOs.SeasonDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminEpisodeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminEpisodeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task LoadSeasonInfoAsync(int seasonId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:44319/api/seasons/{seasonId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var season = JsonConvert.DeserializeObject<ResultSeasonDto>(json);
                    if (season != null)
                    {
                        ViewBag.SeasonNumber = season.SeasonNumber;
                        ViewBag.SeriesID = season.SeriesID;
                    }
                }
            }
            catch
            {
                // Fallback gracefully
            }
        }

        public async Task<IActionResult> EpisodeList(int seasonId)
        {
            ViewBag.v1 = "Bölüm Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Sezon Bölümleri";
            ViewBag.SeasonID = seasonId;
            await LoadSeasonInfoAsync(seasonId);

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44319/api/episodes/season/{seasonId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultEpisodeDto>>(jsonData) ?? new();
                return View(values);
            }
            return View(new List<ResultEpisodeDto>());
        }

        [HttpGet]
        public async Task<IActionResult> CreateEpisode(int seasonId)
        {
            ViewBag.v1 = "Bölüm Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Bölüm Ekleme";
            ViewBag.SeasonID = seasonId;
            await LoadSeasonInfoAsync(seasonId);

            var dto = new CreateEpisodeDto
            {
                SeasonID = seasonId
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode(CreateEpisodeDto dto)
        {
            ViewBag.SeasonID = dto.SeasonID;
            await LoadSeasonInfoAsync(dto.SeasonID);

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:44319/api/episodes", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("EpisodeList", new { seasonId = dto.SeasonID });
            }

            var errJson = await responseMessage.Content.ReadAsStringAsync();
            ModelState.AddModelError("", errJson);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEpisode(int id)
        {
            ViewBag.v1 = "Bölüm Güncelleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Bölüm Güncelleme";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44319/api/episodes/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var ep = JsonConvert.DeserializeObject<ResultEpisodeDto>(jsonData);
                if (ep != null)
                {
                    ViewBag.SeasonID = ep.SeasonID;
                    await LoadSeasonInfoAsync(ep.SeasonID);

                    var updateDto = new UpdateEpisodeDto
                    {
                        EpisodeID = ep.EpisodeID,
                        SeasonID = ep.SeasonID,
                        EpisodeNumber = ep.EpisodeNumber,
                        EpisodeTitle = ep.EpisodeTitle,
                        Overview = ep.Overview,
                        DurationMinutes = ep.DurationMinutes,
                        AirDate = ep.AirDate,
                        StillImageUrl = ep.StillImageUrl
                    };
                    return View(updateDto);
                }
            }
            return RedirectToAction("SeriesList", "AdminSeries");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEpisode(UpdateEpisodeDto dto)
        {
            ViewBag.SeasonID = dto.SeasonID;
            await LoadSeasonInfoAsync(dto.SeasonID);

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:44319/api/episodes", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("EpisodeList", new { seasonId = dto.SeasonID });
            }

            var errJson = await responseMessage.Content.ReadAsStringAsync();
            ModelState.AddModelError("", errJson);
            return View(dto);
        }

        public async Task<IActionResult> DeleteEpisode(int id, int seasonId)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:44319/api/episodes/{id}");
            return RedirectToAction("EpisodeList", new { seasonId = seasonId });
        }
    }
}
