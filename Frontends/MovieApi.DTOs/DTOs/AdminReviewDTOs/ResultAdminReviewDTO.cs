using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.DTOs.DTOs.AdminReviewDTOs
{
    public class ResultAdminReviewDTO
    {
        public int ReviewID { get; set; } //Değerlendirme ID
        public string ReviewComment { get; set; } //Değerlendirme yorumu
        public byte UserRating { get; set; } //Değerlendirme puanı
        public DateTime ReviewDate { get; set; } //Değerlendirme tarihi
        public bool ReviewStatus { get; set; } //Değerlendirme durumu
        public string? UserID { get; set; } // 
        public int MovieID { get; set; } // Film ID'si   
        public bool IsSpoiler { get; set; } // Spoiler İçeriyor mu 
        public int LikeCount { get; set; } // Film kaç defa beğenildi
        public decimal? SentimentScore { get; set; }
    }
}
