using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCastController : Controller
    {
        public IActionResult CastList()
        {
            return View();
        }
    }
}
