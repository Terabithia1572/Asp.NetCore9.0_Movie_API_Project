using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.ViewComponents.UserLayoutWebUIViewComponent
{
    public class _UserLayoutWebUINavbarComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:44319/api";

        public _UserLayoutWebUINavbarComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
