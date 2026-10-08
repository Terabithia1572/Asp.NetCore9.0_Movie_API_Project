using Microsoft.AspNetCore.Mvc;
namespace Movie.Api.UI.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => Redirect("/");
    public IActionResult Error() { Response.StatusCode = 500; ViewData["Message"] = "An unexpected error occurred. Please try again."; return View("~/Views/Flix/Unavailable.cshtml"); }
}
