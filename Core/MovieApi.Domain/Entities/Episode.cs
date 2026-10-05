using System;

namespace MovieApi.Domain.Entities
{
    public class Episode
    {
        public int EpisodeID { get; set; }
        public int SeasonID { get; set; }
        public int EpisodeNumber { get; set; }
        public string EpisodeTitle { get; set; } = null!;
        public string? Overview { get; set; }
        public int? DurationMinutes { get; set; }
        public DateTime? AirDate { get; set; }
        public string? StillImageUrl { get; set; }

        public Season Season { get; set; } = null!;
    }
}
