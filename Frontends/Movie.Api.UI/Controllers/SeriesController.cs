using Microsoft.AspNetCore.Mvc;
namespace Movie.Api.UI.Controllers;
public class SeriesController : Controller
{
    public IActionResult Index() => RedirectToAction(nameof(SeriesList));
    public IActionResult SeriesList(int page = 1, int pageSize = 18) => RedirectToAction("Category", "Flix", new { kind = "series", page });
    public IActionResult SeriesDetail(int id) => RedirectToAction("Series", "Flix", new { id });
}
