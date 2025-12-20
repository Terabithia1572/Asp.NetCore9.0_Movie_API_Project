using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
