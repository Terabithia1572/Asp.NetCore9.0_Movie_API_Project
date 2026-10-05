using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Domain.Entities
{
    public class Series
    {
        public int SeriesID { get; set; } //Dizi ID
        public string SeriesTitle { get; set; } //Dizi adı
        public string SeriesCoverImageURL { get; set; } //Dizi kapak resmi
        public decimal SeriesRating { get; set; } //Dizi puanı
        public string SeriesDescription { get; set; } //Dizi açıklaması
        public DateTime FirstAirDate {  get; set; } //Dizi ilk yayın tarihi
        public string SeriesCreatedYear { get; set; } //Dizi çıkış yılı
        public int? SeriesAverageEpisodeDuration {  get; set; } //Dizi ortalama bölüm süresi
        public int  SeriesSeasonCount {  get; set; } //Dizi sezon sayısı
        public int SeriesEpisodeCount {  get; set; } //Dizi bölüm sayısı
        public bool SeriesStatus { get; set; } //Dizi durumu
        public int CategoryID { get; set; } //Kategori ID
        public Category Category { get; set; } //Kategori nesnesi 
        public ICollection<Season> Seasons { get; set; } = new List<Season>();
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
        public ICollection<SeriesCast> SeriesCasts { get; set; } = new List<SeriesCast>();
        public ICollection<SeriesTag> SeriesTags { get; set; } = new List<SeriesTag>();
    }
}
