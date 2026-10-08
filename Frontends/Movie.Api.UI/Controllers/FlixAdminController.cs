using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using Movie.Api.UI.Services;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminMovieDTOs;
using MovieApi.DTOs.DTOs.AdminReviewDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.DashboardDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;
using MovieApi.DTOs.DTOs.UserDTOs;
using MovieApi.Persistence.Identity;

namespace Movie.Api.UI.Controllers;

[Authorize(Roles = "Admin")]
public class FlixAdminController(MovieApiClient api, FlixCatalogService catalog, UserManager<AppUser> users) : Controller
{
    [HttpGet("/admin/flix")][HttpGet("/admin/index.html")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var stats = await api.GetAsync<ResultDashboardStatsDto>("Dashboard/stats") ?? new();
            return View(new FlixAdminViewModel { MovieCount = stats.TotalMovies, SeriesCount = stats.TotalSeries, ReviewCount = stats.TotalReviews, UserCount = stats.TotalUsers,
                Items = (await catalog.CatalogAsync(sort: "rating", includeUnpublished: true)).Items.Take(5).ToList(),
                Users = (await api.ListAsync<ResultUserDto>("Users")).Take(5).ToList(), Reviews = await api.ListAsync<ResultAdminReviewDTO>("Reviews?pageSize=5") });
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return Unavailable("Index"); }
    }

    [HttpGet("/admin/flix/catalog")][HttpGet("/admin/catalog.html")]
    public async Task<IActionResult> Catalog(string? query, int page = 1, string sort = "newest")
    {
        try
        {
            var result = await catalog.CatalogAsync(query: query, page: page, sort: sort, includeUnpublished: true, pageSize: 20);
            return View(new FlixAdminViewModel { Title = "Catalog", Query = query, Sort = result.Sort, Page = result.Page, Total = result.Total, Items = result.Items });
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return Unavailable("Catalog"); }
    }

    [HttpGet("/admin/flix/users")][HttpGet("/admin/users.html")]
    public async Task<IActionResult> Users(string? query, int page = 1)
    {
        try
        {
            var all = (await api.ListAsync<ResultUserDto>("Users")).Where(u => string.IsNullOrWhiteSpace(query) || (u.UserName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) || (u.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) == true)).OrderBy(u => u.UserName).ToList();
            var model = new FlixAdminViewModel { Title = "Users", Query = query, Total = all.Count };
            model.Page = Math.Clamp(page, 1, model.Pages);
            model.Users = all.Skip((model.Page - 1) * 20).Take(20).ToList();
            return View(model);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return Unavailable("Users"); }
    }

    [HttpGet("/admin/flix/reviews")][HttpGet("/admin/reviews.html")]
    public Task<IActionResult> Reviews(int page = 1, string? query = null) => Feedback("Reviews", page, query);
    [HttpGet("/admin/flix/comments")][HttpGet("/admin/comments.html")]
    public Task<IActionResult> Comments(int page = 1, string? query = null) => Feedback("Comments", page, query);
    private async Task<IActionResult> Feedback(string view, int page, string? query)
    {
        try
        {
            page = Math.Max(1, page);
            using var response = await api.Client.GetAsync($"Reviews?page={page}&pageSize=20&query={Uri.EscapeDataString(query ?? "")}");
            response.EnsureSuccessStatusCode();
            var total = response.Headers.TryGetValues("X-Total-Count", out var values) && int.TryParse(values.FirstOrDefault(), out var count) ? count : 0;
            var pages = Math.Max(1, (int)Math.Ceiling(total / 20d));
            if (page > pages) return RedirectToAction(view, new { page = pages, query });
            return View(view, new FlixAdminViewModel { Title = view, Page = page, Query = query, Total = total, Reviews = await response.Content.ReadFromJsonAsync<List<ResultAdminReviewDTO>>() ?? [] });
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return Unavailable(view); }
    }

    [HttpGet("/admin/flix/add-item")][HttpGet("/admin/add-item.html")]
    public async Task<IActionResult> AddItem()
    {
        var model = new FlixItemInput();
        await LoadOptions(model);
        return View(model);
    }
    [HttpPost("/admin/flix/add-item")][ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(FlixItemInput model)
    {
        await LoadOptions(model);
        if (!model.Categories.Any(x => x.CategoryID == model.CategoryId)) ModelState.AddModelError(nameof(model.CategoryId), "Select an existing category.");
        if (model.CastIds.Except(model.Cast.Select(x => x.CastID)).Any() || model.TagIds.Except(model.Tags.Select(x => x.TagID)).Any()) ModelState.AddModelError("", "Select existing cast and tags.");
        if (!string.IsNullOrWhiteSpace(model.ImageUrl) && FlixCatalogService.ImageUrl(model.ImageUrl) == "/images/poster-placeholder.svg") ModelState.AddModelError(nameof(model.ImageUrl), "Use an HTTP(S) image URL or an absolute site path.");
        if (!ModelState.IsValid) return View(model);
        try
        {
            using var response = model.Kind == "movie"
                ? await api.Client.PostAsJsonAsync("Movies", new AdminCreateMovieDTO { MovieTitle = model.Title, MovieDescription = model.Description, MovieCoverImageURL = model.ImageUrl ?? "", MovieRating = model.Rating, MovieDuration = model.Duration, MovileCreatedYear = model.Year.ToString(), MovieReleaseDate = model.ReleaseDate, CategoryID = model.CategoryId, MovieStatus = model.Published, SelectedCastIds = model.CastIds, SelectedTagIds = model.TagIds })
                : await api.Client.PostAsJsonAsync("Series", new AdminCreateSeriesDTO { SeriesTitle = model.Title, SeriesDescription = model.Description, SeriesCoverImageURL = model.ImageUrl ?? "", SeriesRating = model.Rating, SeriesAverageEpisodeDuration = model.Duration, SeriesCreatedYear = model.Year.ToString(), FirstAirDate = model.ReleaseDate, CategoryID = model.CategoryId, SeriesStatus = model.Published, SelectedCastIds = model.CastIds, SelectedTagIds = model.TagIds });
            if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Catalog));
            ModelState.AddModelError("", "The API could not save this item. Please check the fields.");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { ModelState.AddModelError("", "The catalog service is unavailable; the item could not be saved."); }
        return View(model);
    }
    private async Task LoadOptions(FlixItemInput model)
    {
        try
        {
            model.Categories = await api.ListAsync<AdminResultCategoryDTO>("Categories");
            model.Cast = await api.ListAsync<ResultCastDto>("Casts");
            model.Tags = await api.ListAsync<ResultTagDto>("Tags");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { ModelState.AddModelError("", "Catalog options are unavailable. Saving is disabled until the API is available."); }
    }

    [HttpGet("/admin/flix/edit-user/{id}")][HttpGet("/admin/edit-user.html")]
    public async Task<IActionResult> EditUser(string? id)
    {
        if (string.IsNullOrWhiteSpace(id)) return RedirectToAction(nameof(Users));
        try
        {
            var user = await users.FindByIdAsync(id);
            if (user == null) return NotFound();
            var model = new FlixEditUserInput { Id = user.Id, Username = user.UserName ?? "", Name = user.Name, Surname = user.Surname, Email = user.Email ?? "", PhoneNumber = user.PhoneNumber };
            model.IsLockedOut = user.LockoutEnd > DateTimeOffset.UtcNow;
            model.Reviews = await api.ListAsync<ResultAdminReviewDTO>($"Reviews?userId={Uri.EscapeDataString(id)}&pageSize=100");
            return View(model);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or Microsoft.Data.SqlClient.SqlException) { return Unavailable("Unavailable"); }
    }
    [HttpPost("/admin/flix/edit-user/{id}")][ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(string id, FlixEditUserInput model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        var user = await users.FindByIdAsync(id);
        if (user == null) return NotFound();
        user.Name = model.Name; user.Surname = model.Surname; user.Email = model.Email; user.PhoneNumber = model.PhoneNumber;
        var result = await users.UpdateAsync(user);
        if (result.Succeeded) return RedirectToAction(nameof(Users));
        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(model);
    }
    [AllowAnonymous][HttpGet("/admin/404.html")]
    public IActionResult NotFoundPage() { Response.StatusCode = 404; return View("NotFound"); }
    private IActionResult Unavailable(string view)
    {
        Response.StatusCode = 503;
        return View(view, new FlixAdminViewModel { Title = view, Error = "The administration data is temporarily unavailable. Please try again shortly." });
    }
}
