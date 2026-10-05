using System;

namespace MovieApi.Domain.Entities
{
    public class UserFavorite
    {
        public int UserFavoriteID { get; set; }
        public string UserId { get; set; } = null!;
        public int? MovieID { get; set; }
        public int? SeriesID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Movie? Movie { get; set; }
        public Series? Series { get; set; }
    }
}
