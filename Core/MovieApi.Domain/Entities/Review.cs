using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Domain.Entities
{
    public class Review
    {
        public int ReviewID { get; set; } //Değerlendirme ID
        public string? ReviewComment { get; set; } //Değerlendirme yorumu
        public int? UserRating { get; set; } //Değerlendirme puanı
        public DateTime? ReviewDate { get; set; } //Değerlendirme tarihi
        public bool? ReviewStatus { get; set; } //Değerlendirme durumu
        public string? UserID { get; set; } // 
        public int? MovieID { get; set; } // A review belongs to a movie or a series in the existing database.
        public Movie? Movie { get; set; }
        public int? SeriesID { get; set; }
        public Series? Series { get; set; }
        public bool? IsSpoiler { get; set; } // Spoiler İçeriyor mu 
        public int? LikeCount { get; set; } // Film kaç defa beğenildi
        public decimal? SentimentScore { get; set; }
    }
}
