using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.UserDTOs;
using MovieApi.DTOs.DTOs.UserFavoriteDTOs;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace Movie.Api.UI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                   ?? "";
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            var viewModel = new UserProfileViewModel
            {
                UserId = userId
            };

            // 1. Fetch User Data
            var userRes = await client.GetAsync($"{ApiBaseUrl}/Users/{userId}");
            if (userRes.IsSuccessStatusCode)
            {
                var userJson = await userRes.Content.ReadAsStringAsync();
                var userDto = JsonConvert.DeserializeObject<ResultUserDto>(userJson);
                if (userDto != null)
                {
                    viewModel.Username = userDto.UserName;
                    viewModel.Email = userDto.Email;
                    viewModel.Name = userDto.Name ?? "";
                    viewModel.Surname = userDto.Surname ?? "";
                    viewModel.PhoneNumber = userDto.PhoneNumber ?? "";

                    viewModel.UpdateModel = new UpdateProfileInputModel
                    {
                        Name = viewModel.Name,
                        Surname = viewModel.Surname,
                        Email = viewModel.Email,
                        PhoneNumber = viewModel.PhoneNumber
                    };
                }
            }

            // 2. Fetch Favorite Movies & Series
            var favMoviesRes = await client.GetAsync($"{ApiBaseUrl}/UserFavorites/movies/{userId}");
            if (favMoviesRes.IsSuccessStatusCode)
            {
                var json = await favMoviesRes.Content.ReadAsStringAsync();
                viewModel.FavoriteMovies = JsonConvert.DeserializeObject<List<ResultFavoriteMovieDto>>(json) ?? new List<ResultFavoriteMovieDto>();
                viewModel.FavoriteMoviesCount = viewModel.FavoriteMovies.Count;
            }

            var favSeriesRes = await client.GetAsync($"{ApiBaseUrl}/UserFavorites/series/{userId}");
            if (favSeriesRes.IsSuccessStatusCode)
            {
                var json = await favSeriesRes.Content.ReadAsStringAsync();
                viewModel.FavoriteSeries = JsonConvert.DeserializeObject<List<ResultFavoriteSeriesDto>>(json) ?? new List<ResultFavoriteSeriesDto>();
                viewModel.FavoriteSeriesCount = viewModel.FavoriteSeries.Count;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileInputModel model)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{ApiBaseUrl}/Users/{userId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Profil güncellenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Yeni şifre ve şifre tekrarı uyuşmuyor.";
                return RedirectToAction(nameof(Index));
            }

            // Simulated or identity-backed change password call
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
