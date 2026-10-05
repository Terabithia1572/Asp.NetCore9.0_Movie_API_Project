using System;

namespace MovieApi.Application.Features.CQRSDesignPattern.Results.SeasonResults
{
    public class GetSeasonQueryResult
    {
        public int SeasonID { get; set; }
        public int SeriesID { get; set; }
        public int SeasonNumber { get; set; }
        public string? Overview { get; set; }
        public DateTime? AirDate { get; set; }
        public string? PosterImageUrl { get; set; }
    }
}
