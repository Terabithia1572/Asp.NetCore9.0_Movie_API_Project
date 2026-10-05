using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.UserFavoriteResults;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class GetUserFavoriteMoviesQueryHandler
    {
        private readonly MovieContext _context;

        public GetUserFavoriteMoviesQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetUserFavoriteMoviesQueryResult>> Handle(GetUserFavoriteMoviesQuery query)
        {
            var favorites = await _context.UserFavorites
                .Include(uf => uf.Movie)
                .Where(uf => uf.UserId == query.UserId && uf.MovieID != null)
                .ToListAsync();

            return favorites
                .Where(uf => uf.Movie != null)
                .Select(uf => new GetUserFavoriteMoviesQueryResult
                {
                    UserFavoriteID = uf.UserFavoriteID,
                    UserId = uf.UserId,
                    MovieID = uf.MovieID!.Value,
                    CreatedDate = uf.CreatedDate,
                    MovieTitle = uf.Movie!.MovieTitle,
                    MovieCoverImageURL = uf.Movie.MovieCoverImageURL,
                    MovieRating = uf.Movie.MovieRating,
                    MovieDescription = uf.Movie.MovieDescription,
                    MovieDuration = uf.Movie.MovieDuration,
                    MovieReleaseDate = uf.Movie.MovieReleaseDate,
                    MovileCreatedYear = uf.Movie.MovileCreatedYear,
                    CategoryID = uf.Movie.CategoryID
                }).ToList();
        }
    }
}
