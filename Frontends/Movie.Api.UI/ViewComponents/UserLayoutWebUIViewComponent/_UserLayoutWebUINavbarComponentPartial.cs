using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.ViewComponents.UserLayoutWebUIViewComponent
{
    public class _UserLayoutWebUINavbarComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public _UserLayoutWebUINavbarComponentPartial(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/Categories");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<AdminResultCategoryDTO>>(json) ?? new List<AdminResultCategoryDTO>();
                return View(categories);
            }
            return View(new List<AdminResultCategoryDTO>());
        }
    }
}
