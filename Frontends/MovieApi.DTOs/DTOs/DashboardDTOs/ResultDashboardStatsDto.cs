using System;
using System.Collections.Generic;

namespace MovieApi.DTOs.DTOs.DashboardDTOs
{
    public class ResultDashboardStatsDto
    {
        public int TotalMovies { get; set; }
        public int TotalSeries { get; set; }
        public int TotalCategories { get; set; }
        public int TotalCasts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalReviews { get; set; }

        public List<TopRatedMovieDto> TopRatedMovies { get; set; } = new();
        public List<TopRatedSeriesDto> TopRatedSeries { get; set; } = new();
        public List<LatestReviewDto> LatestReviews { get; set; } = new();
    }

    public class TopRatedMovieDto
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; } = null!;
        public string MovieCoverImageURL { get; set; } = null!;
        public decimal MovieRating { get; set; }
        public string MovileCreatedYear { get; set; } = null!;
    }

    public class TopRatedSeriesDto
    {
        public int SeriesID { get; set; }
        public string SeriesTitle { get; set; } = null!;
        public string SeriesCoverImageURL { get; set; } = null!;
        public decimal SeriesRating { get; set; }
        public string SeriesCreatedYear { get; set; } = null!;
    }

    public class LatestReviewDto
    {
        public int ReviewID { get; set; }
        public string UserComment { get; set; } = null!;
        public decimal UserRating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int? MovieID { get; set; }
        public int? SeriesID { get; set; }
        public string MovieTitle { get; set; } = null!;
    }
}
