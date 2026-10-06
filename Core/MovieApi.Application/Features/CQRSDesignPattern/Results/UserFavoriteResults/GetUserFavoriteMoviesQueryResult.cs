using System;

namespace MovieApi.Application.Features.CQRSDesignPattern.Results.UserFavoriteResults
{
    public class GetUserFavoriteMoviesQueryResult
    {
        public int UserFavoriteID { get; set; }
        public string UserId { get; set; } = null!;
        public int MovieID { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? MovieTitle { get; set; }
        public string? MovieCoverImageURL { get; set; }
        public decimal? MovieRating { get; set; }
        public string? MovieDescription { get; set; }
        public int? MovieDuration { get; set; }
        public DateTime? MovieReleaseDate { get; set; }
        public string? MovileCreatedYear { get; set; }
        public int CategoryID { get; set; }
    }
}
