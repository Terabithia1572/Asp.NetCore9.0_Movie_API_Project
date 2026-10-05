using System;
using System.Collections.Generic;

namespace MovieApi.Domain.Entities
{
    public class Season
    {
        public int SeasonID { get; set; }
        public int SeriesID { get; set; }
        public int SeasonNumber { get; set; }
        public string? Overview { get; set; }
        public DateTime? AirDate { get; set; }
        public string? PosterImageUrl { get; set; }

        public Series Series { get; set; } = null!;
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    }
}
