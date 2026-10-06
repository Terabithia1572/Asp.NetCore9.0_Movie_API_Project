using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("MovieList", "Movie");
        }
    }
}
