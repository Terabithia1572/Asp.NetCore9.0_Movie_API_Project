using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.v1 = "Hakkımızda";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Hakkımızda";
            return View();
        }
    }
}
