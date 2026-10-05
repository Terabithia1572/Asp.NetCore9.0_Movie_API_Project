using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class IsMovieFavoritedQueryHandler
    {
        private readonly MovieContext _context;

        public IsMovieFavoritedQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(IsMovieFavoritedQuery query)
        {
            return await _context.UserFavorites
                .AnyAsync(uf => uf.UserId == query.UserId && uf.MovieID == query.MovieId);
        }
    }
}
