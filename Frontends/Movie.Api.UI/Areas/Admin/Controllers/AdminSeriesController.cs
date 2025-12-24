using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using Newtonsoft.Json;

namespace Series.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSeriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSeriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        private async Task LoadCategoriesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44319/api/Categories");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.CategoryList = new List<SelectListItem>();
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var cats = JsonConvert.DeserializeObject<List<AdminResultCategoryDTO>>(json) ?? new();

            ViewBag.CategoryList = cats
                .OrderBy(x => x.CategoryName)
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryID.ToString()
                })
                .ToList();
        }

        public async Task<IActionResult> SeriesList()
        {
            ViewBag.v1 = "Dizi Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Diziler";

            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var responseMessage = await client.GetAsync("https://localhost:44319/api/Series"); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // gelen veriyi string olarak okuyoruz
                var values = JsonConvert.DeserializeObject<List<AdminResultSeriesDTO>>(jsonData); // gelen veriyi deserialize ediyoruz burada jsonData daki verileri ile ResultSeriesDTO eşleşiyor
                return View(values); // gelen veriyi view'e gönderiyoruz

            }
            return View();
        }
        [HttpGet]
        public async Task< IActionResult> CreateSeries()
        {
            ViewBag.v1 = "Dizi Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Dizi Ekleme";
            await LoadCategoriesAsync();
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateSeries(AdminCreateSeriesDTO adminCreateSeriesDTO)
        {
            // kategori listesi postta da lazım (validation fail olursa sayfa boş kalmasın)
            await LoadCategoriesAsync();

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(adminCreateSeriesDTO);
            var stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:44319/api/Series", stringContent);

            if (responseMessage.IsSuccessStatusCode)
                return RedirectToAction("SeriesList");

            return View(adminCreateSeriesDTO);
        }
        public async Task<IActionResult> DeleteSeries(int id)
        {
            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var responseMessage = await client.DeleteAsync($"https://localhost:44319/api/Series/{id}"); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode) // istek başarılı ise
            {
                return RedirectToAction("SeriesList"); // SeriesList action'ına yönlendiriyoruz
            }
            return View(); // istek başarısız ise aynı sayfada kalıyoruz
        }
    }
}
