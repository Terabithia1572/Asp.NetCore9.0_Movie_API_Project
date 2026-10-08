using Microsoft.AspNetCore.Mvc;
namespace Movie.Api.UI.Controllers;
public class CategoryController : Controller
{
    public IActionResult Index(int? id) => RedirectToAction("Category", "Flix", new { id });
}
