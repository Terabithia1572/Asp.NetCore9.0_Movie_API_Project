using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class UserWebUILayoutController : Controller
    {
        public IActionResult LayoutUI()
        {
            return View();
        }
    }
}
