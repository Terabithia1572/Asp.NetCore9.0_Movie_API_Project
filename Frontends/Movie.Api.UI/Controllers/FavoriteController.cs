using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Services;
using MovieApi.DTOs.DTOs.UserFavoriteDTOs;
using System.Security.Claims;

namespace Movie.Api.UI.Controllers;

[Authorize]
public class FavoriteController(MovieApiClient api) : Controller
{
    public IActionResult Index() => LocalRedirect("/profile#tab-2");

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> ToggleMovieFavorite([FromQuery] int movieId, [FromBody] ToggleFavoriteDto? dto) =>
        Toggle("movie", movieId != 0 ? movieId : dto?.MovieId ?? 0);
    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> ToggleSeriesFavorite([FromQuery] int seriesId, [FromBody] ToggleFavoriteDto? dto) =>
        Toggle("series", seriesId != 0 ? seriesId : dto?.SeriesId ?? 0);

    private async Task<IActionResult> Toggle(string kind, int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Challenge();
        if (id <= 0) return BadRequest(new { success = false, message = "Invalid title." });
        try
        {
            object payload = kind == "movie" ? new { userId, movieId = id } : new { userId, seriesId = id };
            using var response = await api.Client.PostAsJsonAsync($"UserFavorites/toggle-{kind}", payload);
            if (!response.IsSuccessStatusCode) return BadRequest(new { success = false, message = "Unable to update favorites." });
            var result = await response.Content.ReadFromJsonAsync<FavoriteResult>();
            if (result == null) return StatusCode(502, new { success = false, message = "Invalid response from favorites service." });
            return Json(new { success = true, isFavorited = result.IsFavorited });
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        { return StatusCode(503, new { success = false, message = "Favorites service is temporarily unavailable." }); }
    }

    [HttpGet] public Task<IActionResult> CheckMovieFavoriteStatus(int movieId) => Check("movie", movieId);
    [HttpGet] public Task<IActionResult> CheckSeriesFavoriteStatus(int seriesId) => Check("series", seriesId);
    private async Task<IActionResult> Check(string kind, int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Challenge();
        try { return Json(new { isFavorited = await api.GetAsync<bool>($"UserFavorites/is-{kind}-favorited?userId={Uri.EscapeDataString(userId)}&{kind}Id={id}") }); }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return StatusCode(503); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFavorite(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Challenge();
        try
        {
            var movies = await api.ListAsync<ResultFavoriteMovieDto>($"UserFavorites/movies/{Uri.EscapeDataString(userId)}");
            var series = await api.ListAsync<ResultFavoriteSeriesDto>($"UserFavorites/series/{Uri.EscapeDataString(userId)}");
            if (!movies.Any(x => x.UserFavoriteID == id) && !series.Any(x => x.UserFavoriteID == id)) return NotFound();
            using var response = await api.Client.DeleteAsync($"UserFavorites/{id}");
            return response.IsSuccessStatusCode ? Json(new { success = true }) : StatusCode(502);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return StatusCode(503); }
    }
    private sealed class FavoriteResult { public bool IsFavorited { get; set; } }
}
