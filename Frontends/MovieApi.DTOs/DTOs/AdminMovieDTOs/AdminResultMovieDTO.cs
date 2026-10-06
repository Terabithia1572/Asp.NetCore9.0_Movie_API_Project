using System;

namespace MovieApi.DTOs.DTOs.AdminMovieDTOs
{
    public class AdminResultMovieDTO
    {
        public int MovieID { get; set; } //Film ID
        public string? MovieTitle { get; set; } //Film adı
        public string? MovieCoverImageURL { get; set; } //Film kapak resmi
        public decimal? MovieRating { get; set; } //Film puanı
        public string? MovieDescription { get; set; } //Film açıklaması
        public int? MovieDuration { get; set; } //Film süresi
        public DateTime? MovieReleaseDate { get; set; } //Film yayın tarihi
        public string? MovileCreatedYear { get; set; } //Film çıkış yılı
        public bool MovieStatus { get; set; } //Film durumu
        public int CategoryID { get; set; } // Filmin Kategori ID'si
        public string? CategoryName { get; set; } // Filmin Kategori adı
    }
}
