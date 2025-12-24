using MovieApi.Application.Features.CQRSDesignPattern.Commands.MovieCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers
{
    public class UpdateMovieCommandHandler
    {
        private readonly MovieContext _context;

        public UpdateMovieCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateMovieCommand updateMovieCommand)
        {
            var values = await _context.Movies.FindAsync(updateMovieCommand.MovieID);
            values.MovieStatus = updateMovieCommand.MovieStatus;
            values.MovieRating = updateMovieCommand.MovieRating;
            values.MovieDuration = updateMovieCommand.MovieDuration;
            values.MovieDescription = updateMovieCommand.MovieDescription;
            values.MovieTitle = updateMovieCommand.MovieTitle;
            values.MovieReleaseDate = updateMovieCommand.MovieReleaseDate;
            values.MovieCoverImageURL = updateMovieCommand.MovieCoverImageURL;
            values.MovileCreatedYear = updateMovieCommand.MovileCreatedYear;
            await _context.SaveChangesAsync();
        }
    }
}
