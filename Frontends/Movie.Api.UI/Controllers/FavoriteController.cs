using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.UserFavoriteDTOs;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace Movie.Api.UI.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api/UserFavorites";

        public FavoriteController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst("sub")?.Value
                      ?? User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                userId = "1"; // Fallback for unauthenticated/demo testing
            }
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Favorilerim";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Favorilerim";

            var userId = GetUserId();
            var model = new UserFavoriteViewModel();
            var client = _httpClientFactory.CreateClient();

            try
            {
                // Fetch Favorite Movies
                var moviesResponse = await client.GetAsync($"{ApiBaseUrl}/movies/{userId}");
                if (moviesResponse.IsSuccessStatusCode)
                {
                    var moviesJson = await moviesResponse.Content.ReadAsStringAsync();
                    model.FavoriteMovies = JsonConvert.DeserializeObject<List<ResultFavoriteMovieDto>>(moviesJson) ?? new();
                }

                // Fetch Favorite Series
                var seriesResponse = await client.GetAsync($"{ApiBaseUrl}/series/{userId}");
                if (seriesResponse.IsSuccessStatusCode)
                {
                    var seriesJson = await seriesResponse.Content.ReadAsStringAsync();
                    model.FavoriteSeries = JsonConvert.DeserializeObject<List<ResultFavoriteSeriesDto>>(seriesJson) ?? new();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Favoriler yüklenirken bir hata oluştu: " + ex.Message;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMovieFavorite([FromQuery] int movieId, [FromBody] ToggleFavoriteDto? dto)
        {
            var targetMovieId = movieId != 0 ? movieId : (dto?.MovieId ?? 0);
            if (targetMovieId == 0)
            {
                return Json(new { success = false, message = "Geçersiz Film ID" });
            }

            var userId = GetUserId();
            var client = _httpClientFactory.CreateClient();

            var payload = new { userId = userId, movieId = targetMovieId };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{ApiBaseUrl}/toggle-movie", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    bool isFavorited = result?.isFavorited ?? false;
                    return Json(new { success = true, isFavorited = isFavorited });
                }
                return Json(new { success = false, message = "Favori işlemi başarısız." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSeriesFavorite([FromQuery] int seriesId, [FromBody] ToggleFavoriteDto? dto)
        {
            var targetSeriesId = seriesId != 0 ? seriesId : (dto?.SeriesId ?? 0);
            if (targetSeriesId == 0)
            {
                return Json(new { success = false, message = "Geçersiz Dizi ID" });
            }

            var userId = GetUserId();
            var client = _httpClientFactory.CreateClient();

            var payload = new { userId = userId, seriesId = targetSeriesId };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{ApiBaseUrl}/toggle-series", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    bool isFavorited = result?.isFavorited ?? false;
                    return Json(new { success = true, isFavorited = isFavorited });
                }
                return Json(new { success = false, message = "Favori işlemi başarısız." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckMovieFavoriteStatus(int movieId)
        {
            var userId = GetUserId();
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/is-movie-favorited?userId={userId}&movieId={movieId}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    bool isFavorited = JsonConvert.DeserializeObject<bool>(jsonString);
                    return Json(new { isFavorited = isFavorited });
                }
            }
            catch
            {
                // Fallback on error
            }

            return Json(new { isFavorited = false });
        }

        [HttpGet]
        public async Task<IActionResult> CheckSeriesFavoriteStatus(int seriesId)
        {
            var userId = GetUserId();
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/is-series-favorited?userId={userId}&seriesId={seriesId}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    bool isFavorited = JsonConvert.DeserializeObject<bool>(jsonString);
                    return Json(new { isFavorited = isFavorited });
                }
            }
            catch
            {
                // Fallback on error
            }

            return Json(new { isFavorited = false });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.DeleteAsync($"{ApiBaseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

            return Json(new { success = false, message = "Silme işlemi başarısız." });
        }
    }
}
