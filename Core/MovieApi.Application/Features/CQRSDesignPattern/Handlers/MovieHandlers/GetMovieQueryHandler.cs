using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.QCRSDesignPattern.Results.MovieResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers
{
    public class GetMovieQueryHandler
    {
        private readonly MovieContext _context;

        public GetMovieQueryHandler(MovieContext context)
        {
            _context = context;
        }
        public async Task<List<GetMovieQueryResult>> Handle()
        {
            var values = await _context.Movies.ToListAsync();
            return values.Select(x => new GetMovieQueryResult
            {
                MovieID = x.MovieID,
                MovieTitle = x.MovieTitle,
                MovieCoverImageURL = x.MovieCoverImageURL,
                MovieRating = x.MovieRating,
                MovieDescription = x.MovieDescription,
                MovieDuration = x.MovieDuration,
                MovieReleaseDate = x.MovieReleaseDate,
                MovileCreatedYear = x.MovileCreatedYear,
                MovieStatus = x.MovieStatus
            }).ToList();
        }
        }
}
