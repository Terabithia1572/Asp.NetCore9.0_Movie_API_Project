using System;
using System.Collections.Generic;

namespace MovieApi.DTOs.DTOs.AdminMovieDTOs
{
    public class AdminUpdateMovieDTO
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; } = null!;
        public string MovieCoverImageURL { get; set; } = null!;
        public decimal MovieRating { get; set; }
        public string MovieDescription { get; set; } = null!;
        public int MovieDuration { get; set; }
        public DateTime MovieReleaseDate { get; set; }
        public string MovileCreatedYear { get; set; } = null!;
        public bool MovieStatus { get; set; }
        public int CategoryID { get; set; }
        public List<int> SelectedCastIds { get; set; } = new();
        public List<int> SelectedTagIds { get; set; } = new();
    }
}
