using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.EpisodeDTOs;
using MovieApi.DTOs.DTOs.SeasonDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;
using Newtonsoft.Json;
using System.Security.Claims;

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
                var values = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(json) ?? new List<AdminResultSeriesDTO>();
                return View(values);
            }
            return View(new List<AdminResultSeriesDTO>());
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(SeriesList));
        }

        public async Task<IActionResult> SeriesDetail(int id)
        {
            ViewBag.id = id;
            ViewBag.SeriesID = id;

            var client = _httpClientFactory.CreateClient();
            var viewModel = new SeriesDetailViewModel();

            // 1. Fetch Series Detail
            var seriesRes = await client.GetAsync($"{ApiBaseUrl}/Series/GetMovile?id={id}");
            if (seriesRes.IsSuccessStatusCode)
            {
                var json = await seriesRes.Content.ReadAsStringAsync();
                viewModel.Series = JsonConvert.DeserializeObject<AdminResultSeriesDTO>(json) ?? new AdminResultSeriesDTO();
            }

            // Fetch Category Name
            if (viewModel.Series != null && viewModel.Series.CategoryID > 0)
            {
                var catRes = await client.GetAsync($"{ApiBaseUrl}/Categories/GetCategory?id={viewModel.Series.CategoryID}");
                if (catRes.IsSuccessStatusCode)
                {
                    var catJson = await catRes.Content.ReadAsStringAsync();
                    var catObj = JsonConvert.DeserializeObject<AdminResultCategoryDTO>(catJson);
                    if (catObj != null) viewModel.CategoryName = catObj.CategoryName;
                }
            }

            // 2. Fetch Seasons
            var seasonsRes = await client.GetAsync($"{ApiBaseUrl}/seasons/series/{id}");
            if (seasonsRes.IsSuccessStatusCode)
            {
                var seasonsJson = await seasonsRes.Content.ReadAsStringAsync();
                viewModel.Seasons = JsonConvert.DeserializeObject<List<ResultSeasonDto>>(seasonsJson) ?? new List<ResultSeasonDto>();
            }

            // 3. Fetch Episodes for each Season
            foreach (var season in viewModel.Seasons)
            {
                var epRes = await client.GetAsync($"{ApiBaseUrl}/episodes/season/{season.SeasonID}");
                if (epRes.IsSuccessStatusCode)
                {
                    var epJson = await epRes.Content.ReadAsStringAsync();
                    var episodes = JsonConvert.DeserializeObject<List<ResultEpisodeDto>>(epJson) ?? new List<ResultEpisodeDto>();
                    viewModel.EpisodesBySeason[season.SeasonID] = episodes;
                }
            }

            // 4. Fetch Assigned Casts & Tags
            var castIdsRes = await client.GetAsync($"{ApiBaseUrl}/series/{id}/casts");
            if (castIdsRes.IsSuccessStatusCode)
            {
                var castIdsJson = await castIdsRes.Content.ReadAsStringAsync();
                var castIds = JsonConvert.DeserializeObject<List<int>>(castIdsJson);
                if (castIds != null && castIds.Count > 0)
                {
                    var allCastsRes = await client.GetAsync($"{ApiBaseUrl}/Casts");
                    if (allCastsRes.IsSuccessStatusCode)
                    {
                        var allCastsJson = await allCastsRes.Content.ReadAsStringAsync();
                        var allCasts = JsonConvert.DeserializeObject<List<ResultCastDto>>(allCastsJson) ?? new List<ResultCastDto>();
                        viewModel.Casts = allCasts.Where(c => castIds.Contains(c.CastID)).ToList();
                    }
                }
            }

            var tagIdsRes = await client.GetAsync($"{ApiBaseUrl}/series/{id}/tags");
            if (tagIdsRes.IsSuccessStatusCode)
            {
                var tagIdsJson = await tagIdsRes.Content.ReadAsStringAsync();
                var tagIds = JsonConvert.DeserializeObject<List<int>>(tagIdsJson);
                if (tagIds != null && tagIds.Count > 0)
                {
                    var allTagsRes = await client.GetAsync($"{ApiBaseUrl}/Tags");
                    if (allTagsRes.IsSuccessStatusCode)
                    {
                        var allTagsJson = await allTagsRes.Content.ReadAsStringAsync();
                        var allTags = JsonConvert.DeserializeObject<List<ResultTagDto>>(allTagsJson) ?? new List<ResultTagDto>();
                        viewModel.Tags = allTags.Where(t => tagIds.Contains(t.TagID)).ToList();
                    }
                }
            }

            // 5. Check Favorite Status if User Authenticated
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var favRes = await client.GetAsync($"{ApiBaseUrl}/UserFavorites/is-series-favorited?userId={userId}&seriesId={id}");
                if (favRes.IsSuccessStatusCode)
                {
                    var favJson = await favRes.Content.ReadAsStringAsync();
                    if (bool.TryParse(favJson, out var isFav))
                    {
                        viewModel.IsFavorited = isFav;
                    }
                }
            }

            return View(viewModel);
        }
    }
}
