using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Results.MovieResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers
{
    public class GetMovieWithCategoryQueryHandler
    {
        private readonly MovieContext _context;
        public GetMovieWithCategoryQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetMovieWithCategoryQueryResult>> Handle()
        {
            var values = await _context.Movies.Include(x => x.Category).ToListAsync();
            return values.Select(x => new GetMovieWithCategoryQueryResult
            {
                MovieID = x.MovieID,
                MovieTitle = x.MovieTitle,
                MovieCoverImageURL = x.MovieCoverImageURL,
                MovieRating = x.MovieRating,
                MovieDescription = x.MovieDescription,
                MovieDuration = x.MovieDuration,
                MovieReleaseDate = x.MovieReleaseDate,
                MovileCreatedYear = x.MovileCreatedYear,
                MovieStatus = x.MovieStatus,
                CategoryID = x.Category.CategoryID,
                CategoryName = x.Category.CategoryName
            }).ToList();
        }
    }
}
