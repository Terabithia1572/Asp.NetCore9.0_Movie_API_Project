using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminMovieDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminRatingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public AdminRatingController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
        }

        public async Task<IActionResult> RatingList()
        {
            ViewBag.v1 = "Puanlama & Sıralama";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "İçerik Puanları";

            var client = _httpClientFactory.CreateClient();
            var model = new RatingListViewModel();

            try
            {
                var movieRes = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovieWithCategory");
                if (movieRes.IsSuccessStatusCode)
                {
                    var json = await movieRes.Content.ReadAsStringAsync();
                    model.Movies = JsonConvert.DeserializeObject<List<AdminResultMovieDTO>>(json) ?? new();
                    model.Movies = model.Movies.OrderByDescending(m => m.MovieRating).ToList();
                }
            }
            catch { }

            try
            {
                var seriesRes = await client.GetAsync($"{ApiBaseUrl}/Series");
                if (seriesRes.IsSuccessStatusCode)
                {
                    var json = await seriesRes.Content.ReadAsStringAsync();
                    model.Series = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(json) ?? new();
                    model.Series = model.Series.OrderByDescending(s => s.SeriesRating).ToList();
                }
            }
            catch { }

            return View(model);
        }
    }

    public class RatingListViewModel
    {
        public List<AdminResultMovieDTO> Movies { get; set; } = new();
        public List<AdminResultSeriesDTO> Series { get; set; } = new();
    }
}
