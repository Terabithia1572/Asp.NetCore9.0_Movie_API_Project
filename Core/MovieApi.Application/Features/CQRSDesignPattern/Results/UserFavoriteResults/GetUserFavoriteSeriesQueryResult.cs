using System;

namespace MovieApi.Application.Features.CQRSDesignPattern.Results.UserFavoriteResults
{
    public class GetUserFavoriteSeriesQueryResult
    {
        public int UserFavoriteID { get; set; }
        public string UserId { get; set; } = null!;
        public int SeriesID { get; set; }
        public DateTime CreatedDate { get; set; }

        public string SeriesTitle { get; set; } = null!;
        public string SeriesCoverImageURL { get; set; } = null!;
        public decimal SeriesRating { get; set; }
        public string SeriesDescription { get; set; } = null!;
        public DateTime FirstAirDate { get; set; }
        public string SeriesCreatedYear { get; set; } = null!;
        public int? SeriesAverageEpisodeDuration { get; set; }
        public int SeriesSeasonCount { get; set; }
        public int SeriesEpisodeCount { get; set; }
        public int CategoryID { get; set; }
    }
}
