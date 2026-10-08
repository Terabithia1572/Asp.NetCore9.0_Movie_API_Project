using Microsoft.AspNetCore.Mvc;
namespace Movie.Api.UI.Controllers;
public class MovieController : Controller
{
    public IActionResult MovieList(int page = 1, int pageSize = 18) => RedirectToAction("Category", "Flix", new { kind = "movie", page });
    public IActionResult Search(string? query, int page = 1, int pageSize = 18) => RedirectToAction("Category", "Flix", new { query, page });
    public IActionResult MovieDetail(int id) => RedirectToAction("Details", "Flix", new { id });
}
