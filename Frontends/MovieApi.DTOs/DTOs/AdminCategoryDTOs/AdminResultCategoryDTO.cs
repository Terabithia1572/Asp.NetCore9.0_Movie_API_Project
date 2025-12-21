using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.DTOs.DTOs.AdminCategoryDTOs
{
    public class AdminResultCategoryDTO
    {
        public int CategoryID { get; set; } //hangi kategoriyi getireceksek onun id'si
        public string CategoryName { get; set; } //hangi kategoriyi getireceksek onun adı
        public int MovieCount { get; set; } //o kategoride kaç film var
        public int ReviewCount { get; set; } //o kategorideki filmlere kaç tane yorum yapılmış
        public double AvgRating { get; set; } //o kategorideki filmlerin ortalama puanı
        public bool IsActive { get; set; } //o kategorinin aktif mi pasif mi olduğunu gösterir
    }
}
