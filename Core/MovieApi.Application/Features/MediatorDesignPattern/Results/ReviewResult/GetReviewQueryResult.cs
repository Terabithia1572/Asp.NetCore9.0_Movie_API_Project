using MovieApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Results.ReviewResult
{
    public class GetReviewQueryResult
    {
        public int ReviewID { get; set; } //Değerlendirme ID
        public string? ReviewComment { get; set; } //Değerlendirme yorumu
        public int? UserRating { get; set; } //Değerlendirme puanı
        public DateTime? ReviewDate { get; set; } //Değerlendirme tarihi
        public bool? ReviewStatus { get; set; } //Değerlendirme durumu
        public string? UserID { get; set; } // 
        public int? MovieID { get; set; } // Film ID'si
        public int? SeriesID { get; set; }
        public Movie? Movie { get; set; }
        public bool? IsSpoiler { get; set; } // Spoiler İçeriyor mu 
        public int? LikeCount { get; set; } // Film kaç defa beğenildi
        public decimal? SentimentScore { get; set; }
    }
}
