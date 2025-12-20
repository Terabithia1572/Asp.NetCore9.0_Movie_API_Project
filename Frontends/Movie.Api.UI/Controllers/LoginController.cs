using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
