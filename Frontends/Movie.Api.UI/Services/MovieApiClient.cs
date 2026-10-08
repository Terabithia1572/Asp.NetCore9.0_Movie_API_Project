using System.Net;
using System.Net.Http.Json;

namespace Movie.Api.UI.Services;

// Transport only: all catalog data still comes from the existing Web API and its handlers.
public class MovieApiClient(IHttpClientFactory factory)
{
    public HttpClient Client => factory.CreateClient("MovieApi");
    public async Task<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        using var response = await Client.GetAsync(path, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound) return default;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }
    public async Task<List<T>> ListAsync<T>(string path, CancellationToken cancellationToken = default) =>
        await GetAsync<List<T>>(path, cancellationToken) ?? [];
}
