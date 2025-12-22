using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; set; } //Kategori ID
        public string CategoryName { get; set; } //Kategori adı
        public string? CategoryDescription { get; set; } //Kategori açıklaması
        public bool CategoryStatus { get; set; } //Kategori durumu
        public List<Movie> Movies { get; set; } //Kategoriye ait filmler
        public List<Series> Series { get; set; } //Kategoriye ait diziler

    }
}
