using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using Movie.Api.UI.Services;
using System.Security.Claims;

namespace Movie.Api.UI.Controllers;

public class FlixController(FlixCatalogService catalog, ILogger<FlixController> logger) : Controller
{
    [HttpGet("/")]
    [HttpGet("/index.html")]
    public async Task<IActionResult> Index() => await CatalogView("Index");

    [HttpGet("/catalog")]
    [HttpGet("/catalog.html")]
    public async Task<IActionResult> Catalog() => await CatalogView("Catalog");

    [HttpGet("/category/{id:int?}")]
    [HttpGet("/category.html")]
    public async Task<IActionResult> Category(int? id, string? query, string? kind, int? categoryId, string? year, string sort = "newest", int page = 1)
        => await CatalogView("Category", query, kind, id ?? categoryId, year, sort, page);

    private async Task<IActionResult> CatalogView(string view, string? query = null, string? kind = null, int? category = null, string? year = null, string sort = "newest", int page = 1)
    {
        try
        {
            var model = await catalog.CatalogAsync(query, kind, category, year, sort, page);
            if (category.HasValue && !model.Categories.Any(c => c.CategoryID == category)) return NotFound();
            return View(view, model);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning("Catalog API unavailable ({Type}).", ex.GetType().Name);
            Response.StatusCode = 503;
            return View(view, new FlixCatalogViewModel { Error = "The catalog is temporarily unavailable. Please try again shortly." });
        }
    }

    [HttpGet("/details/{id:int}")]
    public Task<IActionResult> Details(int id) => Detail(id, "movie");
    [HttpGet("/series/{id:int}")]
    public Task<IActionResult> Series(int id) => Detail(id, "series");
    [HttpGet("/details.html")]
    [HttpGet("/main/details.html")]
    public IActionResult DetailsLink(int? id) => id.HasValue ? RedirectToAction(nameof(Details), new { id }) : Redirect("/category?kind=movie");

    private async Task<IActionResult> Detail(int id, string kind)
    {
        if (id < 1) return NotFound();
        try
        {
            var model = await catalog.DetailAsync(id, kind, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return model == null ? NotFound() : View("Details", model);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            Response.StatusCode = 503;
            ViewData["Message"] = "This title is temporarily unavailable. Please try again shortly.";
            return View("Unavailable");
        }
    }

    [HttpGet("/about")][HttpGet("/about.html")]
    public IActionResult About() => View();
    [HttpGet("/contacts")][HttpGet("/contacts.html")]
    public IActionResult Contacts() => View();
    [HttpGet("/privacy")][HttpGet("/privacy.html")]
    public IActionResult Privacy() => View();
    [HttpGet("/interview")][HttpGet("/interview.html")]
    public IActionResult Interview() => View();
}
