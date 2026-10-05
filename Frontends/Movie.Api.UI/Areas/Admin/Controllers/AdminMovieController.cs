using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminMovieDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMovieController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminMovieController(IHttpClientFactory httpClientFactory)
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
                    ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
                    ViewBag.CategoryList = categories.Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.CategoryName }).ToList();
                }
            }
            catch { }

            // Load Casts
            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Casts");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var casts = JsonConvert.DeserializeObject<List<ResultCastDto>>(json) ?? new();
                    ViewBag.CastList = casts.Select(c => new SelectListItem { Value = c.CastID.ToString(), Text = $"{c.CastName} {c.CastSurname}".Trim() }).ToList();
                }
            }
            catch { }

            // Load Tags
            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/Tags");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tags = JsonConvert.DeserializeObject<List<ResultTagDto>>(json) ?? new();
                    ViewBag.TagList = tags.Select(t => new SelectListItem { Value = t.TagID.ToString(), Text = t.TagTitle }).ToList();
                }
            }
            catch { }
        }

        public async Task<IActionResult> MovieList()
        {
            ViewBag.v1 = "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovieWithCategory");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<AdminResultMovieDTO>>(jsonData);
                return View(values);
            }
            return View(new List<AdminResultMovieDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> CreateMovie()
        {
            ViewBag.v1 = "Film Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Film Ekleme";

            await LoadCategoriesCastsAndTagsAsync();
            return View(new AdminCreateMovieDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(AdminCreateMovieDTO adminCreateMovieDTO)
        {
            await LoadCategoriesCastsAndTagsAsync();
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(adminCreateMovieDTO);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync($"{ApiBaseUrl}/Movies", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MovieList");
            }
            return View(adminCreateMovieDTO);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMovie(int id)
        {
            ViewBag.v1 = "Film Güncelleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Film Güncelleme";

            await LoadCategoriesCastsAndTagsAsync();
            var client = _httpClientFactory.CreateClient();

            // Get Movie Data
            var movieResponse = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovile?id={id}");
            if (!movieResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("MovieList");
            }

            var movieJson = await movieResponse.Content.ReadAsStringAsync();
            var updateDto = JsonConvert.DeserializeObject<AdminUpdateMovieDTO>(movieJson) ?? new AdminUpdateMovieDTO();

            // Get Assigned Cast IDs
            var castsResponse = await client.GetAsync($"{ApiBaseUrl}/Movies/{id}/casts");
            if (castsResponse.IsSuccessStatusCode)
            {
                var castsJson = await castsResponse.Content.ReadAsStringAsync();
                updateDto.SelectedCastIds = JsonConvert.DeserializeObject<List<int>>(castsJson) ?? new();
            }

            // Get Assigned Tag IDs
            var tagsResponse = await client.GetAsync($"{ApiBaseUrl}/Movies/{id}/tags");
            if (tagsResponse.IsSuccessStatusCode)
            {
                var tagsJson = await tagsResponse.Content.ReadAsStringAsync();
                updateDto.SelectedTagIds = JsonConvert.DeserializeObject<List<int>>(tagsJson) ?? new();
            }

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovie(AdminUpdateMovieDTO adminUpdateMovieDTO)
        {
            await LoadCategoriesCastsAndTagsAsync();
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(adminUpdateMovieDTO);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync($"{ApiBaseUrl}/Movies", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MovieList");
            }
            return View(adminUpdateMovieDTO);
        }

        public async Task<IActionResult> DeleteMovie(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"{ApiBaseUrl}/Movies/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MovieList");
            }
            return RedirectToAction("MovieList");
        }
    }
}
