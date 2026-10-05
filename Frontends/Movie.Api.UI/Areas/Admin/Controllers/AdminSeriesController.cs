using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Series.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSeriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminSeriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task LoadCategoriesCastsAndTagsAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Load Categories
            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Categories");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<AdminResultCategoryDTO>>(json) ?? new();
                    ViewBag.CategoryList = categories
                        .OrderBy(x => x.CategoryName)
                        .Select(x => new SelectListItem { Value = x.CategoryID.ToString(), Text = x.CategoryName })
                        .ToList();
                }
            }
            catch
            {
                ViewBag.CategoryList = new List<SelectListItem>();
            }

            // Load Casts
            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Casts");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var casts = JsonConvert.DeserializeObject<List<ResultCastDto>>(json) ?? new();
                    ViewBag.CastList = casts
                        .OrderBy(x => x.CastName)
                        .Select(c => new SelectListItem { Value = c.CastID.ToString(), Text = $"{c.CastName} {c.CastSurname}".Trim() })
                        .ToList();
                }
            }
            catch
            {
                ViewBag.CastList = new List<SelectListItem>();
            }

            // Load Tags
            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Tags");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tags = JsonConvert.DeserializeObject<List<ResultTagDto>>(json) ?? new();
                    ViewBag.TagList = tags
                        .OrderBy(x => x.TagTitle)
                        .Select(t => new SelectListItem { Value = t.TagID.ToString(), Text = t.TagTitle })
                        .ToList();
                }
            }
            catch
            {
                ViewBag.TagList = new List<SelectListItem>();
            }
        }

        public async Task<IActionResult> SeriesList()
        {
            ViewBag.v1 = "Dizi Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Diziler";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{ApiBaseUrl}/Series");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(jsonData);
                return View(values);
            }
            return View(new List<AdminResultSeriesDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> CreateSeries()
        {
            ViewBag.v1 = "Dizi Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Dizi Ekleme";

            await LoadCategoriesCastsAndTagsAsync();
            return View(new AdminCreateSeriesDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeries(AdminCreateSeriesDTO adminCreateSeriesDTO)
        {
            await LoadCategoriesCastsAndTagsAsync();
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(adminCreateSeriesDTO);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync($"{ApiBaseUrl}/Series", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("SeriesList");
            }

            return View(adminCreateSeriesDTO);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSeries(int id)
        {
            ViewBag.v1 = "Dizi Güncelleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Dizi Güncelleme";

            await LoadCategoriesCastsAndTagsAsync();
            var client = _httpClientFactory.CreateClient();

            // Fetch Series Data
            var seriesResponse = await client.GetAsync($"{ApiBaseUrl}/Series/GetMovile?id={id}");
            if (!seriesResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("SeriesList");
            }

            var seriesJson = await seriesResponse.Content.ReadAsStringAsync();
            var updateDto = JsonConvert.DeserializeObject<AdminUpdateSeriesDTO>(seriesJson) ?? new AdminUpdateSeriesDTO();

            // Fetch Assigned Cast IDs
            var castsResponse = await client.GetAsync($"{ApiBaseUrl}/Series/{id}/casts");
            if (castsResponse.IsSuccessStatusCode)
            {
                var castsJson = await castsResponse.Content.ReadAsStringAsync();
                updateDto.SelectedCastIds = JsonConvert.DeserializeObject<List<int>>(castsJson) ?? new();
            }

            // Fetch Assigned Tag IDs
            var tagsResponse = await client.GetAsync($"{ApiBaseUrl}/Series/{id}/tags");
            if (tagsResponse.IsSuccessStatusCode)
            {
                var tagsJson = await tagsResponse.Content.ReadAsStringAsync();
                updateDto.SelectedTagIds = JsonConvert.DeserializeObject<List<int>>(tagsJson) ?? new();
            }

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeries(AdminUpdateSeriesDTO adminUpdateSeriesDTO)
        {
            await LoadCategoriesCastsAndTagsAsync();
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(adminUpdateSeriesDTO);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync($"{ApiBaseUrl}/Series", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("SeriesList");
            }

            return View(adminUpdateSeriesDTO);
        }

        public async Task<IActionResult> DeleteSeries(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"{ApiBaseUrl}/Series/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("SeriesList");
            }
            return RedirectToAction("SeriesList");
        }
    }
}
