using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using MovieApi.DTOs.DTOs.TagDTOs;
using Newtonsoft.Json;

namespace Movie.Api.UI.ViewComponents.MovieDetailViewComponent
{
    public class _MovieDetailOverviewComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiBaseUrl;

        public _MovieDetailOverviewComponentPartial(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            ApiBaseUrl = (configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/").TrimEnd('/') + "";
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var model = new MovieDetailOverviewViewModel();

            if (id > 0)
            {
                try
                {
                    var movieRes = await client.GetAsync($"{ApiBaseUrl}/Movies/GetMovile?id={id}");
                    if (movieRes.IsSuccessStatusCode)
                    {
                        var json = await movieRes.Content.ReadAsStringAsync();
                        model.Movie = JsonConvert.DeserializeObject<ResultMovieDTO>(json);
                    }
                }
                catch { }

                try
                {
                    var castRes = await client.GetAsync($"{ApiBaseUrl}/Movies/{id}/casts");
                    if (castRes.IsSuccessStatusCode)
                    {
                        var json = await castRes.Content.ReadAsStringAsync();
                        model.Casts = JsonConvert.DeserializeObject<List<ResultCastDto>>(json) ?? new();
                    }
                }
                catch { }

                try
                {
                    var tagRes = await client.GetAsync($"{ApiBaseUrl}/Movies/{id}/tags");
                    if (tagRes.IsSuccessStatusCode)
                    {
                        var json = await tagRes.Content.ReadAsStringAsync();
                        model.Tags = JsonConvert.DeserializeObject<List<ResultTagDto>>(json) ?? new();
                    }
                }
                catch { }
            }

            return View(model);
        }
    }
}
