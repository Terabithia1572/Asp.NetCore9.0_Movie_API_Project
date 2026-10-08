using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/NotFound404")]
        public IActionResult NotFound404()
        {
            // Preserve re-executed 400/403/500 responses; a direct visit is a 404.
            if (Response.StatusCode < 400) Response.StatusCode = 404;
            return View("~/Views/FlixAdmin/NotFound.cshtml");
        }
    }
}
