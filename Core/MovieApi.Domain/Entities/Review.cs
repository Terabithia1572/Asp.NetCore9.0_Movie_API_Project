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
        public string ReviewComment { get; set; } //Değerlendirme yorumu
        public int UserRating { get; set; } //Değerlendirme puanı
        public DateTime ReviewDate { get; set; } //Değerlendirme tarihi
        public bool ReviewStatus { get; set; } //Değerlendirme durumu
    }
}
