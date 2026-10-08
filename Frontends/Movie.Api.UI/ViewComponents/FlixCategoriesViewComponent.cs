using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Services;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;

namespace Movie.Api.UI.ViewComponents;

public class FlixCategoriesViewComponent(MovieApiClient api) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        try { return View(await api.ListAsync<AdminResultCategoryDTO>("Categories")); }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        { return View(new List<AdminResultCategoryDTO>()); }
    }
}
