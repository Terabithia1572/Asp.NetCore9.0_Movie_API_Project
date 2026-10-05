using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminReviewDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminReviewController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminReviewController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ReviewList(int page = 1, int pageSize = 10)
        {
            ViewBag.v1 = "Yorum Yönetimi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Yorumlar";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{ApiBaseUrl}/Reviews?page={page}&pageSize={pageSize}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultAdminReviewDTO>>(jsonData) ?? new();

                var totalCountHeader = responseMessage.Headers.Contains("X-Total-Count")
                    ? responseMessage.Headers.GetValues("X-Total-Count").FirstOrDefault()
                    : null;
                var totalCount = int.TryParse(totalCountHeader, out int tc) ? tc : values.Count;

                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalCount = totalCount;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / Math.Max(1, pageSize));

                return View(values);
            }
            return View(new List<ResultAdminReviewDTO>());
        }

        public async Task<IActionResult> ApproveReview(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");
            await client.PostAsync($"{ApiBaseUrl}/Reviews/approve/{id}", content);
            return RedirectToAction("ReviewList");
        }

        public async Task<IActionResult> DeleteReview(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"{ApiBaseUrl}/Reviews/{id}");
            return RedirectToAction("ReviewList");
        }
    }
}
