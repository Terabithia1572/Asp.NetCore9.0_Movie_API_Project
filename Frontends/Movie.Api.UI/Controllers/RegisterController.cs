using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.UserRegisterDTOs;

namespace Movie.Api.UI.Controllers
{
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserRegisterDTO createUserRegisterDTO)
        {
            return RedirectToAction("SignIn", "Login");
        }
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
