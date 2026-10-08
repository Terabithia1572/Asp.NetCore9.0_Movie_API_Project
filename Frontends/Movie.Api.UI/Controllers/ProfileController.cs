using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using Movie.Api.UI.Services;
using MovieApi.DTOs.DTOs.UserFavoriteDTOs;
using MovieApi.Persistence.Identity;
using System.Security.Claims;

namespace Movie.Api.UI.Controllers;

[Authorize]
public class ProfileController(UserManager<AppUser> users, MovieApiClient api, FlixCatalogService catalog) : Controller
{
    [HttpGet("/profile")][HttpGet("/profile.html")][HttpGet("/Profile/Index")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var user = await users.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null) return Challenge();
            var model = new UserProfileViewModel { UserId = user.Id, Username = user.UserName ?? "", Email = user.Email ?? "", Name = user.Name, Surname = user.Surname, PhoneNumber = user.PhoneNumber ?? "" };
            model.FavoriteMovies = await api.ListAsync<ResultFavoriteMovieDto>($"UserFavorites/movies/{Uri.EscapeDataString(user.Id)}");
            model.FavoriteSeries = await api.ListAsync<ResultFavoriteSeriesDto>($"UserFavorites/series/{Uri.EscapeDataString(user.Id)}");
            model.FavoriteMoviesCount = model.FavoriteMovies.Count;
            model.FavoriteSeriesCount = model.FavoriteSeries.Count;
            model.FavoriteCards = model.FavoriteMovies.Select(m => new FlixCard { Id = m.MovieID, Title = m.MovieTitle, Image = FlixCatalogService.ImageUrl(m.MovieCoverImageURL), Rating = m.MovieRating, Year = m.MovileCreatedYear })
                .Concat(model.FavoriteSeries.Select(s => new FlixCard { Id = s.SeriesID, Kind = "series", Title = s.SeriesTitle, Image = FlixCatalogService.ImageUrl(s.SeriesCoverImageURL), Rating = s.SeriesRating, Year = s.SeriesCreatedYear })).ToList();
            model.Reviews = await api.ListAsync<MovieApi.DTOs.DTOs.AdminReviewDTOs.ResultAdminReviewDTO>($"Reviews?userId={Uri.EscapeDataString(user.Id)}&pageSize=100");
            model.ReviewsCount = model.Reviews.Count;
            model.NewTitles = (await catalog.CatalogAsync()).Items.Take(5).ToList();
            return View("~/Views/Flix/Profile.cshtml", model);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or Microsoft.Data.SqlClient.SqlException)
        {
            Response.StatusCode = 503;
            ViewData["Message"] = "Your profile is temporarily unavailable. Please try again shortly.";
            return View("~/Views/Flix/Unavailable.cshtml");
        }
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UpdateProfileInputModel model)
    {
        if (!ModelState.IsValid) { TempData["ErrorMessage"] = "Please enter a valid name and email address."; return RedirectToAction(nameof(Index)); }
        var user = await users.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (user == null) return Challenge();
        user.Name = model.Name; user.Surname = model.Surname; user.Email = model.Email; user.PhoneNumber = model.PhoneNumber;
        var result = await users.UpdateAsync(user);
        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] = result.Succeeded ? "Profile updated." : string.Join(" ", result.Errors.Select(e => e.Description));
        return RedirectToAction(nameof(Index));
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
    {
        if (!ModelState.IsValid) { TempData["ErrorMessage"] = "Enter your current password and matching new passwords."; return RedirectToAction(nameof(Index)); }
        var user = await users.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (user == null) return Challenge();
        var result = await users.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] = result.Succeeded ? "Password changed." : string.Join(" ", result.Errors.Select(e => e.Description));
        return RedirectToAction(nameof(Index));
    }
}
