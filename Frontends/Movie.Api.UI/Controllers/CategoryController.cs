using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using Newtonsoft.Json;

namespace Movie.Api.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var viewModel = new CategoryDetailViewModel
            {
                CategoryID = id
            };

            // 1. Fetch Category Detail
            var catRes = await client.GetAsync($"{ApiBaseUrl}/Categories/GetCategory?id={id}");
            if (catRes.IsSuccessStatusCode)
            {
                var catJson = await catRes.Content.ReadAsStringAsync();
                var catObj = JsonConvert.DeserializeObject<AdminResultCategoryDTO>(catJson);
                if (catObj != null)
                {
                    viewModel.CategoryName = catObj.CategoryName;
                }
            }

            // 2. Fetch Movies belonging to category
            var moviesRes = await client.GetAsync($"{ApiBaseUrl}/Movies");
            if (moviesRes.IsSuccessStatusCode)
            {
                var moviesJson = await moviesRes.Content.ReadAsStringAsync();
                var allMovies = JsonConvert.DeserializeObject<List<ResultMovieDTO>>(moviesJson) ?? new List<ResultMovieDTO>();
                viewModel.Movies = allMovies.Where(m => m.CategoryID == id).ToList();
            }

            // 3. Fetch Series belonging to category
            var seriesRes = await client.GetAsync($"{ApiBaseUrl}/Series");
            if (seriesRes.IsSuccessStatusCode)
            {
                var seriesJson = await seriesRes.Content.ReadAsStringAsync();
                var allSeries = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(seriesJson) ?? new List<AdminResultSeriesDTO>();
                viewModel.Series = allSeries.Where(s => s.CategoryID == id).ToList();
            }

            ViewBag.CategoryName = viewModel.CategoryName;
            return View(viewModel);
        }
    }
}
