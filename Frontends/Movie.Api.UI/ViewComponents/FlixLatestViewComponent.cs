using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using Movie.Api.UI.Services;
namespace Movie.Api.UI.ViewComponents;

public class FlixLatestViewComponent(FlixCatalogService catalog) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        try { return View((await catalog.CatalogAsync()).Items.Take(4).ToList()); }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return View(new List<FlixCard>()); }
    }
}
