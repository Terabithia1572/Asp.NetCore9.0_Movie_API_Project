using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CategoryList()
        {
            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var responseMessage = await client.GetAsync("https://localhost:44319/api/Categories"); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // gelen veriyi string olarak okuyoruz
                var values = JsonConvert.DeserializeObject<List<AdminResultCategoryDTO>>(jsonData); // gelen veriyi deserialize ediyoruz burada jsonData daki verileri ile ResultMovieDTO eşleşiyor
                return View(values); // gelen veriyi view'e gönderiyoruz

            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(AdminCreateCategoryDTO adminCreateCategoryDTO)
        {
            var client = _httpClientFactory.CreateClient(); //burada client oluşturuyoruz
            var jsonData = JsonConvert.SerializeObject(adminCreateCategoryDTO); // gelen veriyi json formatına çeviriyoruz
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"); // gelen veriyi string olarak gönderiyoruz
            var responseMessage = await client.PostAsync("https://localhost:44319/api/Categories", stringContent); // buraya api url yazılacak istek yapmak istediğimiz URL'ye istek yapılıyor
            if (responseMessage.IsSuccessStatusCode) // istek başarılı ise
            {
                return RedirectToAction("CategoryList"); // CategoryList action'ına yönlendiriyoruz
            }
            return View(); // istek başarısız ise aynı sayfada kalıyoruz
        }
    }
}
