using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/NotFound404")]
        public IActionResult NotFound404()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
