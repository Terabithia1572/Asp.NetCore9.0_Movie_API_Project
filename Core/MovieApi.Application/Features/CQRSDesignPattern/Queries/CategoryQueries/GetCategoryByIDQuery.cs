using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Queries.CategoryQueries
{
    public class GetCategoryByIDQuery
    {
        public GetCategoryByIDQuery(int categoryID)
        {
            CategoryID = categoryID;
        }

        public int CategoryID { get; set; } // Kategori ID
    }
}
