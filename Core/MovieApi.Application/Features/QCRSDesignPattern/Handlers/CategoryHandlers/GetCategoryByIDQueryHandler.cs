using MovieApi.Application.Features.QCRSDesignPattern.Queries.CategoryQueries;
using MovieApi.Application.Features.QCRSDesignPattern.Results.CategoryResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.CategoryHandlers
{
    public class GetCategoryByIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetCategoryByIDQueryHandler(MovieContext context)
        {
            _context = context;
        }
        public async Task<GetCategoryByIDQueryResult> Handle(GetCategoryByIDQuery getCategoryByIDQuery)
        {
            var values = await _context.Categories.FindAsync(getCategoryByIDQuery.CategoryID);
            return new GetCategoryByIDQueryResult
            {
                CategoryID = values.CategoryID,
                CategoryName = values.CategoryName
            };

        }
    }
}
