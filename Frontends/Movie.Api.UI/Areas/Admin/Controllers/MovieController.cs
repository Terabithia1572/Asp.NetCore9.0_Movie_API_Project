using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        public IActionResult MovieList()
        {
            return View();
        }
    }
}
