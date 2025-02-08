using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Domain.Entities
{
    public class Movie
    {
        public int MovieID { get; set; } //Film ID
        public string MovieTitle { get; set; } //Film adı
        public string MovieCoverImageURL { get; set; } //Film kapak resmi
        public decimal MovieRating { get; set; } //Film puanı
        public string MovieDescription { get; set; } //Film açıklaması
        public int MovieDuration { get; set; } //Film süresi
        public DateTime MovieReleaseDate { get; set; } //Film yayın tarihi
        public string MovileCreatedYear { get; set; } //Film çıkış yılı
        public bool MovieStatus { get; set; } //Film durumu
    }
}
