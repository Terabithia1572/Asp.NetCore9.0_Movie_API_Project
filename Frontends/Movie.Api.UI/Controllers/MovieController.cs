using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult MovieList()
        {
            ViewBag.v1= "Film Listesi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Tüm Filmler";
            return View();
        }
    }
}
