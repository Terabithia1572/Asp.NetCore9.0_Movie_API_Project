using System.ComponentModel.DataAnnotations;
using MovieApi.DTOs.DTOs.AdminCategoryDTOs;
using MovieApi.DTOs.DTOs.AdminReviewDTOs;
using MovieApi.DTOs.DTOs.CastDTOs;
using MovieApi.DTOs.DTOs.EpisodeDTOs;
using MovieApi.DTOs.DTOs.SeasonDTOs;
using MovieApi.DTOs.DTOs.TagDTOs;
using MovieApi.DTOs.DTOs.UserDTOs;

namespace Movie.Api.UI.Models;

public class FlixCard
{
    public int Id { get; set; }
    public string Kind { get; set; } = "movie";
    public string Title { get; set; } = "Untitled";
    public string Image { get; set; } = "/images/poster-placeholder.svg";
    public string Description { get; set; } = "No description available.";
    public decimal? Rating { get; set; }
    public string Year { get; set; } = "—";
    public int? Duration { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; } = "Uncategorized";
    public bool Published { get; set; }
    public string Url => Kind == "series" ? $"/series/{Id}" : $"/details/{Id}";
    public string RatingText => Rating?.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) ?? "—";
}

public class FlixCatalogViewModel
{
    public string Title { get; set; } = "Catalog";
    public string? Query { get; set; }
    public string? Kind { get; set; }
    public int? CategoryId { get; set; }
    public string? Year { get; set; }
    public string Sort { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int Total { get; set; }
    public int PageSize { get; set; } = 18;
    public int Pages => Math.Max(1, (int)Math.Ceiling((double)Total / PageSize));
    public List<FlixCard> Items { get; set; } = [];
    public List<FlixCard> Featured { get; set; } = [];
    public List<AdminResultCategoryDTO> Categories { get; set; } = [];
    public Dictionary<int, int> CategoryCounts { get; set; } = [];
    public List<string> Years { get; set; } = [];
    public string? Error { get; set; }
    public string PageUrl(int page) => Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("/category", new Dictionary<string, string?>
    { ["page"] = page.ToString(), ["query"] = Query, ["kind"] = Kind, ["categoryId"] = CategoryId?.ToString(), ["year"] = Year, ["sort"] = Sort });
}

public class FlixDetailViewModel
{
    public FlixCard Item { get; set; } = new();
    public List<FlixCard> Related { get; set; } = [];
    public List<ResultCastDto> Cast { get; set; } = [];
    public List<ResultTagDto> Tags { get; set; } = [];
    public List<ResultAdminReviewDTO> Reviews { get; set; } = [];
    public List<ResultSeasonDto> Seasons { get; set; } = [];
    public Dictionary<int, List<ResultEpisodeDto>> Episodes { get; set; } = [];
    public bool IsFavorite { get; set; }
}

public class FlixAdminViewModel
{
    public string Title { get; set; } = "Dashboard";
    public string? Query { get; set; }
    public string Sort { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int Total { get; set; }
    public int Pages => Math.Max(1, (int)Math.Ceiling(Total / 20d));
    public List<FlixCard> Items { get; set; } = [];
    public List<ResultAdminReviewDTO> Reviews { get; set; } = [];
    public List<ResultUserDto> Users { get; set; } = [];
    public int MovieCount { get; set; }
    public int SeriesCount { get; set; }
    public int ReviewCount { get; set; }
    public int UserCount { get; set; }
    public string? Error { get; set; }
}

public class FlixLoginInput
{
    [Required] public string Username { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}

public class FlixRegisterInput
{
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Required, StringLength(100)] public string Surname { get; set; } = "";
    [Required, StringLength(100)] public string Username { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    [Compare(nameof(Password)), DataType(DataType.Password)] public string ConfirmPassword { get; set; } = "";
    [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the privacy policy.")] public bool AcceptPrivacy { get; set; }
}
