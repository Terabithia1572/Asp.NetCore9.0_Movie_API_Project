using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using MovieApi.DTOs.DTOs.TagDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public MovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> MovieList()
        {
            ViewBag.v1 = "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{ApiBaseUrl}/Movies");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMovieDTO>>(jsonData) ?? new List<ResultMovieDTO>();
                return View(values);
            }
            return View(new List<ResultMovieDTO>());
        }

        public async Task<IActionResult> MovieDetail(int id)
        {
            ViewBag.id = id;
            ViewBag.MovieID = id;

            var client = _httpClientFactory.CreateClient();
            var model = new MovieDetailOverviewViewModel();

            if (id > 0)
            {
                // 1. Fetch Movie Detail
                var movieRes = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovile?id={id}");
                if (movieRes.IsSuccessStatusCode)
                {
                    var json = await movieRes.Content.ReadAsStringAsync();
                    model.Movie = JsonConvert.DeserializeObject<ResultMovieDTO>(json);
                }

                // 2. Fetch Casts
                var castIdsRes = await client.GetAsync($"{ApiBaseUrl}/Movies/{id}/casts");
                if (castIdsRes.IsSuccessStatusCode)
                {
                    var castIdsJson = await castIdsRes.Content.ReadAsStringAsync();
                    var castIds = JsonConvert.DeserializeObject<List<int>>(castIdsJson);
                    if (castIds != null && castIds.Any())
                    {
                        var allCastsRes = await client.GetAsync($"{ApiBaseUrl}/Casts");
                        if (allCastsRes.IsSuccessStatusCode)
                        {
                            var allCastsJson = await allCastsRes.Content.ReadAsStringAsync();
                            var allCasts = JsonConvert.DeserializeObject<List<ResultCastDto>>(allCastsJson) ?? new List<ResultCastDto>();
                            model.Casts = allCasts.Where(c => castIds.Contains(c.CastID)).ToList();
                        }
                    }
                }

                // 3. Fetch Tags
                var tagIdsRes = await client.GetAsync($"{ApiBaseUrl}/Movies/{id}/tags");
                if (tagIdsRes.IsSuccessStatusCode)
                {
                    var tagIdsJson = await tagIdsRes.Content.ReadAsStringAsync();
                    var tagIds = JsonConvert.DeserializeObject<List<int>>(tagIdsJson);
                    if (tagIds != null && tagIds.Any())
                    {
                        var allTagsRes = await client.GetAsync($"{ApiBaseUrl}/Tags");
                        if (allTagsRes.IsSuccessStatusCode)
                        {
                            var allTagsJson = await allTagsRes.Content.ReadAsStringAsync();
                            var allTags = JsonConvert.DeserializeObject<List<ResultTagDto>>(allTagsJson) ?? new List<ResultTagDto>();
                            model.Tags = allTags.Where(t => tagIds.Contains(t.TagID)).ToList();
                        }
                    }
                }
            }

            return View(model);
        }
    }
}
