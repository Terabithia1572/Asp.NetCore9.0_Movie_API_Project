using System;

namespace MovieApi.Application.Features.CQRSDesignPattern.Results.UserFavoriteResults
{
    public class GetUserFavoriteMoviesQueryResult
    {
        public int UserFavoriteID { get; set; }
        public string UserId { get; set; } = null!;
        public int MovieID { get; set; }
        public DateTime CreatedDate { get; set; }

        public string MovieTitle { get; set; } = null!;
        public string MovieCoverImageURL { get; set; } = null!;
        public decimal MovieRating { get; set; }
        public string MovieDescription { get; set; } = null!;
        public int MovieDuration { get; set; }
        public DateTime MovieReleaseDate { get; set; }
        public string MovileCreatedYear { get; set; } = null!;
        public int CategoryID { get; set; }
    }
}
