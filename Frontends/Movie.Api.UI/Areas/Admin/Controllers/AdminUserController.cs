using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.UserDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public AdminUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> UserList()
        {
            ViewBag.v1 = "Kullanıcı Yönetimi";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Kullanıcılar";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/Users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<ResultUserDto>>(json) ?? new();
                return View(users);
            }

            return View(new List<ResultUserDto>());
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            ViewBag.v1 = "Kullanıcı Düzenle";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Kullanıcı Güncelleme";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UpdateUserDto>(json);
                return View(user);
            }

            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{ApiBaseUrl}/Users/{dto.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("UserList");
            }

            return View(dto);
        }

        public async Task<IActionResult> ChangeStatus(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");
            await client.PostAsync($"{ApiBaseUrl}/Users/{id}/toggle-status", content);

            return RedirectToAction("UserList");
        }
    }
}
