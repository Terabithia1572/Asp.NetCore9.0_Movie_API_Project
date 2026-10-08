using Microsoft.AspNetCore.Mvc;
namespace Movie.Api.UI.Controllers;
public class AboutController : Controller
{
    public IActionResult Index() => RedirectToAction("About", "Flix");
}
