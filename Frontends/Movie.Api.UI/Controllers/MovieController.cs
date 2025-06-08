using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.MovieDTO;
using Newtonsoft.Json;

namespace Movie.Api.UI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task< IActionResult> MovieList()
        {
            ViewBag.v1= "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";

            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var responseMessage = await client.GetAsync("https://localhost:44319/api/Movies"); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // gelen veriyi string olarak okuyoruz
                var values=JsonConvert.DeserializeObject<List<ResultMovieDTO>>(jsonData); // gelen veriyi deserialize ediyoruz burada jsonData daki verileri ile ResultMovieDTO eşleşiyor
                return View(values); // gelen veriyi view'e gönderiyoruz

            }
            return View();

        }

        public async Task<IActionResult> MovieDetail(int id)
        {
          
            id = 0;
            return View();
        }
    }
}
