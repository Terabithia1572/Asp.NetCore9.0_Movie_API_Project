using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminMovieDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminReportController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Sistem Raporları";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "İçerik & Kullanıcı Raporları";

            var client = _httpClientFactory.CreateClient();
            var model = new AdminReportViewModel();

            try
            {
                var movieRes = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovieWithCategory");
                if (movieRes.IsSuccessStatusCode)
                {
                    var json = await movieRes.Content.ReadAsStringAsync();
                    model.TopMovies = (JsonConvert.DeserializeObject<List<AdminResultMovieDTO>>(json) ?? new())
                        .OrderByDescending(m => m.MovieRating)
                        .Take(10)
                        .ToList();
                }
            }
            catch { }

            try
            {
                var seriesRes = await client.GetAsync($"{ApiBaseUrl}/Series");
                if (seriesRes.IsSuccessStatusCode)
                {
                    var json = await seriesRes.Content.ReadAsStringAsync();
                    model.TopSeries = (JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(json) ?? new())
                        .OrderByDescending(s => s.SeriesRating)
                        .Take(10)
                        .ToList();
                }
            }
            catch { }

            return View(model);
        }

        public async Task<IActionResult> ExportCsv(string reportType)
        {
            var client = _httpClientFactory.CreateClient();
            var sb = new StringBuilder();

            if (reportType == "movies")
            {
                sb.AppendLine("MovieID;MovieTitle;MovieRating;MovileCreatedYear;CategoryName");
                try
                {
                    var res = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovieWithCategory");
                    if (res.IsSuccessStatusCode)
                    {
                        var json = await res.Content.ReadAsStringAsync();
                        var movies = JsonConvert.DeserializeObject<List<AdminResultMovieDTO>>(json) ?? new();
                        foreach (var m in movies)
                        {
                            sb.AppendLine($"{m.MovieID};\"{m.MovieTitle}\";{m.MovieRating};{m.MovileCreatedYear};\"{m.CategoryName}\"");
                        }
                    }
                }
                catch { }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"Movies_Report_{DateTime.Now:yyyyMMdd}.csv");
            }
            else
            {
                sb.AppendLine("SeriesID;SeriesTitle;SeriesRating;SeriesCreatedYear;SeasonCount;EpisodeCount");
                try
                {
                    var res = await client.GetAsync($"{ApiBaseUrl}/Series");
                    if (res.IsSuccessStatusCode)
                    {
                        var json = await res.Content.ReadAsStringAsync();
                        var series = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(json) ?? new();
                        foreach (var s in series)
                        {
                            sb.AppendLine($"{s.SeriesID};\"{s.SeriesTitle}\";{s.SeriesRating};{s.SeriesCreatedYear};{s.SeriesSeasonCount};{s.SeriesEpisodeCount}");
                        }
                    }
                }
                catch { }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"Series_Report_{DateTime.Now:yyyyMMdd}.csv");
            }
        }
    }

    public class AdminReportViewModel
    {
        public List<AdminResultMovieDTO> TopMovies { get; set; } = new();
        public List<AdminResultSeriesDTO> TopSeries { get; set; } = new();
    }
}
