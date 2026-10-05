using System;

namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands
{
    public class CreateEpisodeCommand
    {
        public int SeasonID { get; set; }
        public int EpisodeNumber { get; set; }
        public string EpisodeTitle { get; set; } = null!;
        public string? Overview { get; set; }
        public int? DurationMinutes { get; set; }
        public DateTime? AirDate { get; set; }
        public string? StillImageUrl { get; set; }
    }
}
