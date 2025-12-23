using MovieApi.Application.Features.QCRSDesignPattern.Queries.MovieQueries;
using MovieApi.Application.Features.QCRSDesignPattern.Results.MovieResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers
{
    public class GetMovieByIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetMovieByIDQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<GetMovieByIDQueryResult> Handle(GetMovieByIDQuery getMovieByIDQuery)
        {
            var values=await _context.Movies.FindAsync(getMovieByIDQuery.MovieID);
            return new GetMovieByIDQueryResult
            {
                MovieID=values.MovieID,
                MovieTitle = values.MovieTitle,
                MovieCoverImageURL = values.MovieCoverImageURL,
                MovieRating = values.MovieRating,
                MovieDescription = values.MovieDescription,
                MovieDuration = values.MovieDuration,
                MovieReleaseDate = values.MovieReleaseDate,
                MovileCreatedYear = values.MovileCreatedYear,
                MovieStatus = values.MovieStatus
            };
        }
    }
}
