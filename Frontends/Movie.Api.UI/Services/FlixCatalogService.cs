using Movie.Api.UI.Models;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminReviewDTOs;
using MovieApi.DTOs.DTOs.AdminSeriesDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.EpisodeDTOs;
using MovieApi.DTOs.DTOs.MovieDTO;
using MovieApi.DTOs.DTOs.SeasonDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;

namespace Movie.Api.UI.Services;

public class FlixCatalogService(MovieApiClient api)
{
    public async Task<FlixCatalogViewModel> CatalogAsync(string? query = null, string? kind = null, int? categoryId = null,
        string? year = null, string sort = "newest", int page = 1, bool includeUnpublished = false, int pageSize = 18)
    {
        var categories = await api.ListAsync<AdminResultCategoryDTO>("Categories");
        var movies = await api.ListAsync<ResultMovieDTO>("Movies");
        var series = await api.ListAsync<AdminResultSeriesDTO>("Series");
        var all = movies.Select(m => new FlixCard { Id = m.MovieID, Title = m.MovieTitle ?? "Untitled", Image = ImageUrl(m.MovieCoverImageURL),
            Description = m.MovieDescription ?? "No description available.", Rating = m.MovieRating, Year = m.MovileCreatedYear ?? m.MovieReleaseDate?.Year.ToString() ?? "—",
            Duration = m.MovieDuration, CategoryId = m.CategoryID, Published = m.MovieStatus })
            .Concat(series.Select(s => new FlixCard { Id = s.SeriesID, Kind = "series", Title = s.SeriesTitle ?? "Untitled", Image = ImageUrl(s.SeriesCoverImageURL),
                Description = s.SeriesDescription ?? "No description available.", Rating = s.SeriesRating, Year = s.SeriesCreatedYear ?? s.FirstAirDate?.Year.ToString() ?? "—",
                Duration = s.SeriesAverageEpisodeDuration, CategoryId = s.CategoryID, Published = s.SeriesStatus }))
            .Where(x => includeUnpublished || x.Published).ToList();
        foreach (var item in all) item.Category = categories.FirstOrDefault(c => c.CategoryID == item.CategoryId)?.CategoryName ?? "Uncategorized";
        IEnumerable<FlixCard> filtered = all;
        if (!string.IsNullOrWhiteSpace(query)) filtered = filtered.Where(x => x.Title.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase));
        if (kind is "movie" or "series") filtered = filtered.Where(x => x.Kind == kind);
        if (categoryId.HasValue) filtered = filtered.Where(x => x.CategoryId == categoryId);
        if (!string.IsNullOrWhiteSpace(year)) filtered = filtered.Where(x => x.Year == year);
        filtered = sort switch { "rating" => filtered.OrderByDescending(x => x.Rating).ThenBy(x => x.Title),
            "title" => filtered.OrderBy(x => x.Title), _ => filtered.OrderByDescending(x => x.Year).ThenByDescending(x => x.Id) };
        var list = filtered.ToList();
        var model = new FlixCatalogViewModel { Query = query, Kind = kind, CategoryId = categoryId, Year = year, Sort = sort,
            Total = list.Count, PageSize = Math.Clamp(pageSize, 1, 100), Categories = categories, CategoryCounts = all.GroupBy(x => x.CategoryId).ToDictionary(g => g.Key, g => g.Count()),
            Years = all.Select(x => x.Year).Where(x => x != "—").Distinct().OrderDescending().ToList(),
            Featured = all.OrderByDescending(x => x.Rating).Take(6).ToList() };
        model.Page = Math.Clamp(page, 1, model.Pages);
        model.Items = list.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();
        model.Title = categoryId.HasValue ? categories.FirstOrDefault(c => c.CategoryID == categoryId)?.CategoryName ?? "Category" : kind == "series" ? "TV series" : kind == "movie" ? "Movies" : "Catalog";
        return model;
    }

    public async Task<FlixDetailViewModel?> DetailAsync(int id, string kind, string? userId)
    {
        var catalog = await CatalogAsync(kind: kind);
        // A detail record need not be on the first catalog page.
        var categories = catalog.Categories;
        FlixCard? item;
        if (kind == "series")
        {
            var s = (await api.ListAsync<AdminResultSeriesDTO>("Series")).FirstOrDefault(x => x.SeriesID == id && x.SeriesStatus);
            item = s == null ? null : new FlixCard { Id = id, Kind = kind, Title = s.SeriesTitle ?? "Untitled", Image = ImageUrl(s.SeriesCoverImageURL), Description = s.SeriesDescription ?? "No description available.", Rating = s.SeriesRating, Year = s.SeriesCreatedYear ?? "—", Duration = s.SeriesAverageEpisodeDuration, CategoryId = s.CategoryID };
        }
        else
        {
            var m = (await api.ListAsync<ResultMovieDTO>("Movies")).FirstOrDefault(x => x.MovieID == id && x.MovieStatus);
            item = m == null ? null : new FlixCard { Id = id, Title = m.MovieTitle ?? "Untitled", Image = ImageUrl(m.MovieCoverImageURL), Description = m.MovieDescription ?? "No description available.", Rating = m.MovieRating, Year = m.MovileCreatedYear ?? "—", Duration = m.MovieDuration, CategoryId = m.CategoryID };
        }
        if (item == null) return null;
        item.Category = categories.FirstOrDefault(c => c.CategoryID == item.CategoryId)?.CategoryName ?? "Uncategorized";
        var endpoint = kind == "series" ? "Series" : "Movies";
        var castIds = await api.ListAsync<int>($"{endpoint}/{id}/casts");
        var tagIds = await api.ListAsync<int>($"{endpoint}/{id}/tags");
        var model = new FlixDetailViewModel { Item = item,
            Related = (await CatalogAsync(categoryId: item.CategoryId)).Items.Where(x => x.Id != id || x.Kind != kind).Take(12).ToList(),
            Cast = (await api.ListAsync<ResultCastDto>("Casts")).Where(x => castIds.Contains(x.CastID)).ToList(),
            Tags = (await api.ListAsync<ResultTagDto>("Tags")).Where(x => tagIds.Contains(x.TagID)).ToList() };
        if (kind == "movie") model.Reviews = (await api.ListAsync<ResultAdminReviewDTO>($"Reviews?movieId={id}&pageSize=100")).Where(x => x.ReviewStatus == true).ToList();
        else
        {
            model.Reviews = (await api.ListAsync<ResultAdminReviewDTO>($"Reviews?seriesId={id}&pageSize=100")).Where(x => x.ReviewStatus == true).ToList();
            model.Seasons = (await api.ListAsync<ResultSeasonDto>($"Seasons/series/{id}")).OrderBy(x => x.SeasonNumber).ToList();
            foreach (var season in model.Seasons) model.Episodes[season.SeasonID] = (await api.ListAsync<ResultEpisodeDto>($"Episodes/season/{season.SeasonID}")).OrderBy(x => x.EpisodeNumber).ToList();
        }
        if (!string.IsNullOrEmpty(userId)) model.IsFavorite = await api.GetAsync<bool>($"UserFavorites/is-{kind}-favorited?userId={Uri.EscapeDataString(userId)}&{kind}Id={id}");
        return model;
    }

    public static string ImageUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "/images/poster-placeholder.svg";
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme is "https" or "http") return value;
        if (value.StartsWith('/') && !value.StartsWith("//")) return value;
        return "/images/poster-placeholder.svg";
    }
}
