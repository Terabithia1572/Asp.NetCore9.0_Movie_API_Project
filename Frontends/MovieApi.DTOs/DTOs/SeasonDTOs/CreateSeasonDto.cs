using System;

namespace MovieApi.DTOs.DTOs.SeasonDTOs
{
    public class CreateSeasonDto
    {
        public int SeriesID { get; set; }
        public int SeasonNumber { get; set; }
        public string? Overview { get; set; }
        public DateTime? AirDate { get; set; }
        public string? PosterImageUrl { get; set; }
    }
}
