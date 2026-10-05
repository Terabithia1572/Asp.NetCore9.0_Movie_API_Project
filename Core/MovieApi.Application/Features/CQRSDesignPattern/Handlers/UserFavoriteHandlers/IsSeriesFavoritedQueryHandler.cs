using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class IsSeriesFavoritedQueryHandler
    {
        private readonly MovieContext _context;

        public IsSeriesFavoritedQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(IsSeriesFavoritedQuery query)
        {
            return await _context.UserFavorites
                .AnyAsync(uf => uf.UserId == query.UserId && uf.SeriesID == query.SeriesId);
        }
    }
}
