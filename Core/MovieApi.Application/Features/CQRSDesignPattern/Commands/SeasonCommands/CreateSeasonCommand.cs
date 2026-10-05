using System;

namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands
{
    public class CreateSeasonCommand
    {
        public int SeriesID { get; set; }
        public int SeasonNumber { get; set; }
        public string? Overview { get; set; }
        public DateTime? AirDate { get; set; }
        public string? PosterImageUrl { get; set; }
    }
}
