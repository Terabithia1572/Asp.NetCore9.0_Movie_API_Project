using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.UserRegisterDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory; 

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserRegisterDTO createUserRegisterDTO)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData=JsonConvert.SerializeObject(createUserRegisterDTO);
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:44319/api/Registers", stringContent);

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Kayıt İşlemi Başarısız. Lütfen Tekrar Deneyin.");
                return View(createUserRegisterDTO);
            }
           
        }
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
