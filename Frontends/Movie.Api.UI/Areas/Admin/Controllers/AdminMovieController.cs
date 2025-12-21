using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminMovieDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMovieController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> MovieList()
        {
            ViewBag.v1 = "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";

            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var responseMessage = await client.GetAsync("https://localhost:44319/api/Movies"); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // gelen veriyi string olarak okuyoruz
                var values = JsonConvert.DeserializeObject<List<AdminResultMovieDTO>>(jsonData); // gelen veriyi deserialize ediyoruz burada jsonData daki verileri ile ResultMovieDTO eşleşiyor
                return View(values); // gelen veriyi view'e gönderiyoruz

            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateMovie()
        {
            ViewBag.v1 = "Film Ekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Film Ekleme";
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie(AdminCreateMovieDTO adminCreateMovieDTO)
        {
            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var jsonData = JsonConvert.SerializeObject(adminCreateMovieDTO); // gelen veriyi json formatına çeviriyoruz
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"); // gelen veriyi string olarak gönderiyoruz
            var responseMessage = await client.PostAsync("https://localhost:44319/api/Movies", stringContent); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode) // istek başarılı ise
            {
                return RedirectToAction("MovieList"); // MovieList action'ına yönlendiriyoruz
            }
            return View(); // istek başarısız ise aynı sayfada kalıyoruz
        }
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var responseMessage = await client.DeleteAsync($"https://localhost:44319/api/Movies/{id}"); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode) // istek başarılı ise
            {
                return RedirectToAction("MovieList"); // MovieList action'ına yönlendiriyoruz
            }
            return View(); // istek başarısız ise aynı sayfada kalıyoruz
        }
    }
}
