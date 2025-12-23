using MovieApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Results.SeriesResults
{
    public class GetSeriesQueryResult
    {
        public int SeriesID { get; set; } //Dizi ID
        public string SeriesTitle { get; set; } //Dizi adı
        public string SeriesCoverImageURL { get; set; } //Dizi kapak resmi
        public decimal SeriesRating { get; set; } //Dizi puanı
        public string SeriesDescription { get; set; } //Dizi açıklaması
        public DateTime FirstAirDate { get; set; } //Dizi ilk yayın tarihi
        public string SeriesCreatedYear { get; set; } //Dizi çıkış yılı
        public int? SeriesAverageEpisodeDuration { get; set; } //Dizi ortalama bölüm süresi
        public int SeriesSeasonCount { get; set; } //Dizi sezon sayısı
        public int SeriesEpisodeCount { get; set; } //Dizi bölüm sayısı
        public bool SeriesStatus { get; set; } //Dizi durumu
        public int CategoryID { get; set; } //Kategori ID
       
    }
}
