using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Results.CategoryResults
{
    public class GetCategoryByIDQueryResult
    {
        public int CategoryID { get; set; } //Kategori ID
        public string CategoryName { get; set; } //Kategori adı
    }
}
