using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;

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

        public async Task<IActionResult> SeriesList()
        {
            ViewBag.v1 = "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";

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
        public IActionResult CreateSeries()
        {
            ViewBag.v1 = "Film Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Film Ekleme";
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateSeries(AdminCreateSeriesDTO adminCreateSeriesDTO)
        {
            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var jsonData = JsonConvert.SerializeObject(adminCreateSeriesDTO); // gelen veriyi json formatına çeviriyoruz
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"); // gelen veriyi string olarak gönderiyoruz
            var responseMessage = await client.PostAsync("https://localhost:44319/api/Series", stringContent); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode) // istek başarılı ise
            {
                return RedirectToAction("SeriesList"); // SeriesList action'ına yönlendiriyoruz
            }
            return View(); // istek başarısız ise aynı sayfada kalıyoruz
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
