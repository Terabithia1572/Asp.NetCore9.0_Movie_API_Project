using System;
using System.Collections.Generic;

namespace MovieApi.Domain.Entities
{
    public class Movie
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
        public int CategoryID { get; set; } //Kategori ID
        public Category? Category { get; set; } //Kategori nesnesi 
        public List<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
        public ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();
        public ICollection<MovieTag> MovieTags { get; set; } = new List<MovieTag>();
    }
}
