using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminReviewDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using MovieApi.DTOs.DTOs.TagDTOs;
using Newtonsoft.Json;
using System.Security.Claims;

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

        public async Task<IActionResult> MovieList(int page = 1, int pageSize = 18)
        {
            if (page < 1) page = 1;

            ViewBag.v1 = "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{ApiBaseUrl}/Movies");
            var allMovies = new List<ResultMovieDTO>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                allMovies = JsonConvert.DeserializeObject<List<ResultMovieDTO>>(jsonData) ?? new List<ResultMovieDTO>();
            }

            var totalCount = allMovies.Count;
            var pagedItems = allMovies.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedListViewModel<ResultMovieDTO>
            {
                Items = pagedItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Search(string query, int page = 1, int pageSize = 18)
        {
            if (page < 1) page = 1;

            ViewBag.v1 = "Arama Sonuçları";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Arama";
            ViewBag.Query = query;
            ViewBag.IsSearch = true;

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{ApiBaseUrl}/Movies");
            var allMovies = new List<ResultMovieDTO>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                allMovies = JsonConvert.DeserializeObject<List<ResultMovieDTO>>(jsonData) ?? new List<ResultMovieDTO>();
            }

            if (!string.IsNullOrEmpty(query))
            {
                allMovies = allMovies.Where(x => x.MovieTitle != null && x.MovieTitle.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalCount = allMovies.Count;
            var pagedItems = allMovies.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedListViewModel<ResultMovieDTO>
            {
                Items = pagedItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View("MovieList", viewModel);
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

                if (model.Movie != null)
                {
                    // 2. Fetch Category Name
                    if (model.Movie.CategoryID > 0)
                    {
                        var catRes = await client.GetAsync($"{ApiBaseUrl}/Categories/GetCategory?id={model.Movie.CategoryID}");
                        if (catRes.IsSuccessStatusCode)
                        {
                            var catJson = await catRes.Content.ReadAsStringAsync();
                            var catObj = JsonConvert.DeserializeObject<AdminResultCategoryDTO>(catJson);
                            if (catObj != null) model.CategoryName = catObj.CategoryName;
                        }
                    }

                    // 3. Fetch Related Movies (Same category, excluding current)
                    var allMoviesRes = await client.GetAsync($"{ApiBaseUrl}/Movies");
                    if (allMoviesRes.IsSuccessStatusCode)
                    {
                        var allMoviesJson = await allMoviesRes.Content.ReadAsStringAsync();
                        var allMovies = JsonConvert.DeserializeObject<List<ResultMovieDTO>>(allMoviesJson) ?? new List<ResultMovieDTO>();
                        model.RelatedMovies = allMovies.Where(m => m.CategoryID == model.Movie.CategoryID && m.MovieID != id).Take(6).ToList();
                    }
                }

                // 4. Fetch Casts
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

                // 5. Fetch Tags
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

                // 6. Fetch Reviews
                var reviewsRes = await client.GetAsync($"{ApiBaseUrl}/Reviews");
                if (reviewsRes.IsSuccessStatusCode)
                {
                    var json = await reviewsRes.Content.ReadAsStringAsync();
                    var allReviews = JsonConvert.DeserializeObject<List<ResultAdminReviewDTO>>(json) ?? new List<ResultAdminReviewDTO>();
                    model.Reviews = allReviews.Where(r => r.MovieID == id).ToList();
                }

                // 7. Check Favorite Status if authenticated
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var favRes = await client.GetAsync($"{ApiBaseUrl}/UserFavorites/is-movie-favorited?userId={userId}&movieId={id}");
                    if (favRes.IsSuccessStatusCode)
                    {
                        var favJson = await favRes.Content.ReadAsStringAsync();
                        if (bool.TryParse(favJson, out var isFav))
                        {
                            model.IsFavorited = isFav;
                        }
                    }
                }
            }

            return View(model);
        }
    }
}
